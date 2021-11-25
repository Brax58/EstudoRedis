using Microsoft.Data.SqlClient;

namespace EstudoRedis
{
    public static class Conexao
    {
        private static readonly SqlConnection sqlConnection = new SqlConnection("Persist Security Info = False; User ID = sa; Password=!@12QWqw;Initial Catalog = IoT; Server=127.0.0.1");
        public static SqlConnection Conectar() 
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();

            return sqlConnection;
        }

        public static void Desconectar()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                sqlConnection.Close();
        }
    }
}
