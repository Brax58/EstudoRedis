using System;

namespace EstudoRedis
{
    public class Pessoa
    {
        public string Id { get; private set; }
        public string Name { get; private set;}
        public DateTime DataNascimento { get; private set; }

        public Pessoa()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void SetarDados(string id,string name,DateTime dataNascimento) {
            Id = id;
            Name = name;
            DataNascimento = dataNascimento;
        }
    }
}
