using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstudoRedis
{
    [ApiController]
    [Route("[Controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly IConnectionMultiplexer _distributed;

        public PessoaController(IConnectionMultiplexer distributed)
        {
            _distributed = distributed;

        }

        [HttpPost]
        public void SetardadosRedis([FromBody] Pessoa pessoa)
        {
            var command = new SqlCommand();
            command.Connection = Conexao.Conectar();


            command.CommandText = "insert into Pessoa values(@id,@nome,@data)";
            command.Parameters.AddWithValue("@id", pessoa.Id);
            command.Parameters.AddWithValue("@nome", pessoa.Name);
            command.Parameters.AddWithValue("@data", pessoa.DataNascimento);

            command.ExecuteNonQuery();
            Conexao.Desconectar();
        }

        [HttpGet("{cacheKey}")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> BuscarDadosRedis(string cacheKey)
        {
            var redis = _distributed.GetDatabase();
            var command = new SqlCommand();
            command.Connection = Conexao.Conectar();

            var pessoaJson = await redis.StringGetAsync($"pessoa:{cacheKey}");

            if (pessoaJson.IsNull)
            {
                var pessoa = new Pessoa();
                command.CommandText = "select * from Pessoa where Name = @nome";
                command.Parameters.AddWithValue("@nome", cacheKey);

                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    pessoa.SetarDados(dr[0].ToString(),dr[1].ToString(),DateTime.Parse(dr[2].ToString()));
                }
                await redis.StringSetAsync($"pessoa:{cacheKey}", JsonConvert.SerializeObject(pessoa), TimeSpan.FromMinutes(1));
                Conexao.Desconectar();
                return Ok(pessoa);
            }

            return Ok(JsonConvert.DeserializeObject<Pessoa>(pessoaJson));
        }
    }
}
