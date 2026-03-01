using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionEquipos.Config
{
    public class DbContext
    {
        private readonly string _connectionString;

        public DbContext(string connectionString) 
        {
            _connectionString = connectionString;
        }

        public async Task<DataTable> SeleccionarAdapterAsync(SqlCommand cmd)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    await connection.OpenAsync();
                    adapter.Fill(dt);
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public async Task<DataTable> SeleccionarAsync(SqlCommand cmd)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public async Task<int> EjecutarAsync(SqlCommand cmd, bool retornaIdentity = false)
        {
            int resultado = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;

                await connection.OpenAsync();

                if (retornaIdentity)
                {
                    var resultadoEscalar = await cmd.ExecuteScalarAsync();
                    if (resultadoEscalar != null && resultadoEscalar != DBNull.Value)
                    {
                        resultado = Convert.ToInt32(resultadoEscalar);
                    }
                }
                else
                {
                    resultado = await cmd.ExecuteNonQueryAsync();
                }

                return resultado;
            }
        }

        public async Task<(int TotalCount, DataTable Rows)> SeleccionarMultipleAsync(SqlCommand cmd)
        {
            var dt = new DataTable();
            int totalCount = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await connection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader != null)
                        {
                            if (await reader.ReadAsync())
                            {
                                try
                                {
                                    totalCount = reader["TotalCount"] != DBNull.Value ? Convert.ToInt32(reader["TotalCount"]) : Convert.ToInt32(reader.GetValue(0));
                                }
                                catch
                                {
                                    totalCount = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader.GetValue(0));
                                }
                            }

                            if (await reader.NextResultAsync())
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return (totalCount, dt);
        }
    }
}
