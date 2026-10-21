using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Biblioteca.Interfaces;
using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public class AlumnoRepositorio : IAlumno
    {
        public List<Alumno> ObtenerTodos()
        {
            List<Alumno> lista = new List<Alumno>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();
                //se cambio de alumns a alumnos para que coincida con la tabla de la base de datos
                string query = "SELECT * FROM alumnos ORDER BY apellidos, nombre";

                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Alumno alumno = new Alumno();

                        alumno.ID = Convert.ToInt32(dr["id"].ToString());
                        alumno.Matricula = dr["matricula"].ToString();
                        alumno.Nombre = dr["nombre"].ToString();
                        alumno.Apellidos = dr["apellidos"].ToString();
                        alumno.Carrera = dr["carrera"].ToString();
                        alumno.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;

                        lista.Add(alumno);
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

        public List<Alumno> ObtenerActivos()
        {
            List<Alumno> lista = new List<Alumno>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT * FROM alumnos WHERE activo = 1 ORDER BY apellidos, nombre";

                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Alumno alumno = new Alumno();

                        alumno.ID = Convert.ToInt32(dr["id"].ToString());
                        alumno.Matricula = dr["matricula"].ToString();
                        alumno.Nombre = dr["nombre"].ToString();
                        alumno.Apellidos = dr["apellidos"].ToString();
                        alumno.Carrera = dr["carrera"].ToString();
                        alumno.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;

                        lista.Add(alumno);
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

        public List<Alumno> BuscarPorMatricula(string matricula)
        {
            List<Alumno> lista = new List<Alumno>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT * FROM alumnos WHERE matricula = @matricula";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@matricula", matricula);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Alumno alumno = new Alumno();

                        alumno.ID = Convert.ToInt32(dr["id"].ToString());
                        alumno.Matricula = dr["matricula"].ToString();
                        alumno.Nombre = dr["nombre"].ToString();
                        alumno.Apellidos = dr["apellidos"].ToString();
                        alumno.Carrera = dr["carrera"].ToString();
                        alumno.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;

                        lista.Add(alumno);
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "matricula=" + matricula);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lista;
        }

        public List<Alumno> BuscarPorNombre(string nombre)
        {
            List<Alumno> lista = new List<Alumno>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT * FROM alumnos " +
                               "WHERE CONCAT(nombre, ' ', apellidos LIKE @nombre " +
                               "ORDER BY apellidos, nombre";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@nombre", "%" + nombre + "%");

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Alumno alumno = new Alumno();

                        alumno.ID = Convert.ToInt32(dr["id"].ToString());
                        alumno.Matricula = dr["matricula"].ToString();
                        alumno.Nombre = dr["nombre"].ToString();
                        alumno.Apellidos = dr["apellidos"].ToString();
                        alumno.Carrera = dr["carrera"].ToString();
                        alumno.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;

                        lista.Add(alumno);
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "nombre=" + nombre);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lista;
        }

        public Alumno Buscar(int id)
        {
            Alumno alumno = null;

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT * FROM alumnos WHERE id = @Id";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@1d", id);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.Read())
                {
                    alumno = new Alumno();

                    alumno.ID = Convert.ToInt32(dr["id"].ToString());
                    alumno.Matricula = dr["matricula"].ToString();
                    alumno.Nombre = dr["nombre"].ToString();
                    alumno.Apellidos = dr["apellidos"].ToString();
                    alumno.Carrera = dr["carrera"].ToString();
                    alumno.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;
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

            return alumno;
        }

        public bool Agregar(Alumno alumno)
        {
            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "INSERT INTO alumnos(matricula, nombre, apellidos, carrera, activo) " +
                               "VALUES(@matricula, @nombre, @apellidos, @carrera, @activo)";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@matricula", alumno.Matricula);
                comm.Parameters.AddWithValue("@nombre", alumno.Nombre);
                comm.Parameters.AddWithValue("@apellidos", alumno.Apellidos);
                comm.Parameters.AddWithValue("@carrera", alumno.Carrera);
                comm.Parameters.AddWithValue("@activo", alumno.Activo ? 1 : 0);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "matricula=" + alumno.Matricula + ", nombre=" + alumno.Nombre + ", apellidos=" + alumno.Apellidos + ", carrera=" + alumno.Carrera + ", activo=" + alumno.Activo);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool Modificar(Alumno alumno)
        {
            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "UPDATE alumnos SET matricula = @matricula, nombre = @nombre, " +
                               "apellidos = @apellidos, carrera = @carrera, activo = @activo " +
                               "WHERE id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@matricula", alumno.Matricula);
                comm.Parameters.AddWithValue("@nombre", alumno.Nombre);
                comm.Parameters.AddWithValue("@apellidos", alumno.Apellidos);
                comm.Parameters.AddWithValue("@carrera", alumno.Carrera);
                comm.Parameters.AddWithValue("@activo", alumno.Activo ? 1 : 0);
                comm.Parameters.AddWithValue("@id", alumno.ID); //se quito el + 1 

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "id=" + alumno.ID + ", matricula=" + alumno.Matricula + ", nombre=" + alumno.Nombre + ", apellidos=" + alumno.Apellidos + ", carrera=" + alumno.Carrera + ", activo=" + alumno.Activo);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool Desactivar(int id)
        {
            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "UPDATE alumnos SET activo = 0 WHERE id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@id", id);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
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
        }

        public bool ExisteMatricula(string matricula)
        {
            bool existe = false;

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT id FROM alumnos WHERE matricula = @matricula";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@matricula", matricula);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    existe = true;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "matricula=" + matricula);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return existe;
        }
    }
}