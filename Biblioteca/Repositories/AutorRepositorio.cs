using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Biblioteca.Interfaces;
using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public class AutorRepositorio : IAutor
    {
        public List<Autor> ObtenerTodos()
        {
            List<Autor> lista = new List<Autor>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT * FROM autores ORDER BY apellidos, nombre";

                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Autor autor = new Autor();

                        autor.ID = Convert.ToInt32(dr["id"].ToString());
                        autor.Nombre = dr["nombre"].ToString();
                        autor.Apellidos = dr["apellidos"].ToString();
                        autor.Nacionalidad = dr["nacionalidad"].ToString();

                        lista.Add(autor);
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Sin parametros");
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lista;
        }

        public Autor Buscar(int id)
        {
            Autor autor = null;

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT * FROM autores WHERE id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@id", id);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.Read())
                {
                    autor = new Autor();

                    autor.ID = Convert.ToInt32(dr["id"].ToString());
                    autor.Nombre = dr["nombre"].ToString();
                    autor.Apellidos = dr["apellidos"].ToString();
                    autor.Nacionalidad = dr["nacionalidad"].ToString();
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "id=" + id);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return autor;
        }

        public bool Agregar(Autor autor)
        {
            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "INSERT INTO autores(nombre, apellidos, nacionalidad) " +
                               "VALUES(@nombre, @apellidos, @nacionalidad)";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@nombre", autor.Nombre);
                comm.Parameters.AddWithValue("@apellidos", autor.Apellidos);
                comm.Parameters.AddWithValue("@nacionalidad", autor.Nacionalidad);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "nombre=" + autor.Nombre + ", apellidos=" + autor.Apellidos + ", nacionalidad=" + autor.Nacionalidad);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool Modificar(Autor autor)
        {
            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "UPDATE autores SET nombre = @nombre, apellidos = @apellidos, " +
                               "nacionalidad = @nacionalidad WHERE id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@nombre", autor.Nombre);
                comm.Parameters.AddWithValue("@apellidos", autor.Apellidos);
                comm.Parameters.AddWithValue("@nacionalidad", autor.Nacionalidad);
                comm.Parameters.AddWithValue("@id", autor.ID);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "id=" + autor.ID + ", nombre=" + autor.Nombre + ", apellidos=" + autor.Apellidos + ", nacionalidad=" + autor.Nacionalidad);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }


        public bool Existe(string nombre, string apellidos)
        {
            using (MySqlConnection conn = new MySqlConnection(Utils.connStr))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM autores " +
                               "WHERE LOWER(TRIM(nombre)) = LOWER(TRIM(@nombre)) " +
                               "AND LOWER(TRIM(apellidos)) = LOWER(TRIM(@apellidos))";

                using (MySqlCommand comm = new MySqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", nombre);
                    comm.Parameters.AddWithValue("@apellidos", apellidos);

                    int cantidad = Convert.ToInt32(comm.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

    }
}