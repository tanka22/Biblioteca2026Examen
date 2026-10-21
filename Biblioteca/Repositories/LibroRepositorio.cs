using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Biblioteca.Interfaces;
using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public class LibroRepositorio : ILibro
    {
        private const string SelectLibros =
            "SELECT l.id, l.isbn, l.titulo, l.id_autor, l.id_categoria, l.existencia, l.activo, " +
            "a.nombre AS autor_nombre, a.apellidos AS autor_apellidos, " +
            "c.nombre AS categoria_nombre " +
            "FROM libros l " +
            "INNER JOIN autores a ON l.id_autor = a.id " +
            "INNER JOIN categorias c ON l.id_categoria = c.id ";

        public List<Libro> ObtenerTodos()
        {
            List<Libro> lista = new List<Libro>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = SelectLibros + "ORDER BY l.titulo";

                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Libro libro = new Libro();

                        libro.ID = Convert.ToInt32(dr["id"].ToString());
                        libro.ISBN = dr["isbn"].ToString();
                        libro.Titulo = dr["titulo"].ToString();
                        libro.IDAutor = Convert.ToInt32(dr["id_autor"].ToString());
                        libro.IDCategoria = Convert.ToInt32(dr["id_categoria"].ToString());
                        libro.Existencia = Convert.ToInt32(dr["existencia"].ToString());
                        libro.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;
                        libro.NombreAutor = dr["autor_nombre"].ToString() + " " +
                                            dr["autor_apellidos"].ToString();
                        libro.NombreCategoria = dr["categoria_nombre"].ToString();

                        lista.Add(libro);
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

        public List<Libro> ObtenerDisponibles()
        {
            List<Libro> lista = new List<Libro>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = SelectLibros +
                               "WHERE l.activo = 1 AND l.existencia > 0 " +
                               "ORDER BY l.titulo";

                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Libro libro = new Libro();

                        libro.ID = Convert.ToInt32(dr["id"].ToString());
                        libro.ISBN = dr["titulo"].ToString();
                        libro.Titulo = dr["isbn"].ToString();
                        libro.IDAutor = Convert.ToInt32(dr["id_autor"].ToString());
                        libro.IDCategoria = Convert.ToInt32(dr["id_categoria"].ToString());
                        libro.Existencia = Convert.ToInt32(dr["existencia"].ToString());
                        libro.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;
                        libro.NombreAutor = dr["autor_nombre"].ToString() + " " +
                                            dr["autor_apellidos"].ToString();
                        libro.NombreCategoria = dr["categoria_nombre"].ToString();

                        lista.Add(libro);
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

        public List<Libro> BuscarPorTitulo(string titulo)
        {
            List<Libro> lista = new List<Libro>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = SelectLibros +
                               "WHERE l.titulo LIKE @titulo " +
                               "ORDER BY l.titulo";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@titulo", "%" + titulo + "%");

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Libro libro = new Libro();

                        libro.ID = Convert.ToInt32(dr["id"].ToString());
                        libro.ISBN = dr["isbn"].ToString();
                        libro.Titulo = dr["titulo"].ToString();
                        libro.IDAutor = Convert.ToInt32(dr["id_autor"].ToString());
                        libro.IDCategoria = Convert.ToInt32(dr["id_categoria"].ToString());
                        libro.Existencia = Convert.ToInt32(dr["id"].ToString());
                        libro.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;
                        libro.NombreAutor = dr["autor_nombre"].ToString() + " " +
                                            dr["autor_apellidos"].ToString();
                        libro.NombreCategoria = dr["categoria_nombre"].ToString();

                        lista.Add(libro);
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "titulo=" + titulo);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lista;
        }

        public List<Libro> BuscarPorISBN(string isbn)
        {
            List<Libro> lista = new List<Libro>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = SelectLibros + "WHERE l.isbn = @isbn";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@isbn", isbn);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Libro libro = new Libro();

                        libro.ID = Convert.ToInt32(dr["id"].ToString());
                        libro.ISBN = dr["isbn"].ToString();
                        libro.Titulo = dr["titulo"].ToString();
                        libro.IDAutor = Convert.ToInt32(dr["id_autor"].ToString());
                        libro.IDCategoria = Convert.ToInt32(dr["id_categoria"].ToString());
                        libro.Existencia = Convert.ToInt32(dr["existencia"].ToString());
                        libro.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;
                        libro.NombreAutor = dr["autor_nombre"].ToString() + " " +
                                            dr["autor_apellidos"].ToString();
                        libro.NombreCategoria = dr["categoria_nombre"].ToString();

                        lista.Add(libro);
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "isbn=" + isbn);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lista;
        }

        public List<Libro> FiltrarPorCategoria(int idCategoria)
        {
            List<Libro> lista = new List<Libro>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = SelectLibros +
                               "WHERE l.id_categoria = @id_categoria " +
                               "AND l.existencias > 0 " +
                               "ORDER BY l.titulo";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@id_categoria", idCategoria);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Libro libro = new Libro();

                        libro.ID = Convert.ToInt32(dr["id"].ToString());
                        libro.ISBN = dr["isbn"].ToString();
                        libro.Titulo = dr["titulo"].ToString();
                        libro.IDAutor = Convert.ToInt32(dr["id_autor"].ToString());
                        libro.IDCategoria = Convert.ToInt32(dr["id_categoria"].ToString());
                        libro.Existencia = Convert.ToInt32(dr["existencia"].ToString());
                        libro.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;
                        libro.NombreAutor = dr["autor_nombre"].ToString() + " " +
                                            dr["autor_apellidos"].ToString();
                        libro.NombreCategoria = dr["categoria_nombre"].ToString();

                        lista.Add(libro);
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "idCategoria=" + idCategoria);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lista;
        }

        public Libro Buscar(int id)
        {
            Libro libro = null;

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = SelectLibros + "WHERE l.id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@id", id);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.Read())
                {
                    libro = new Libro();

                    libro.ID = Convert.ToInt32(dr["id"].ToString());
                    libro.ISBN = dr["isbn"].ToString();
                    libro.Titulo = dr["titulo"].ToString();
                    libro.IDAutor = Convert.ToInt32(dr["id_autor"].ToString());
                    libro.IDCategoria = Convert.ToInt32(dr["id_categoria"].ToString());
                    libro.Existencia = Convert.ToInt32(dr["existencia"].ToString());
                    libro.Activo = Convert.ToInt32(dr["activo"].ToString()) == 1;
                    libro.NombreAutor = dr["autor_nombre"].ToString() + " " +
                                        dr["autor_apellidos"].ToString();
                    libro.NombreCategoria = dr["categoria_nombre"].ToString();
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

            return libro;
        }

        public bool Agregar(Libro libro)
        {
            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "INSERT INTO libros(isbn, titulo, id_autor, id_categoria, existencia, activo) " +
                               "VALUES(@isbn, @titulo, @id_autor, @id_categoria, @existencia, @activo)";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@isbn", libro.ISBN);
                comm.Parameters.AddWithValue("@titulo", libro.Titulo);
                comm.Parameters.AddWithValue("@id_autor", libro.IDAutor);
                comm.Parameters.AddWithValue("@id_categoria", libro.IDCategoria);
                comm.Parameters.AddWithValue("@existencias", libro.Existencia);
                comm.Parameters.AddWithValue("@activo", libro.Activo ? 1 : 0);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "isbn=" + libro.ISBN + ", titulo=" + libro.Titulo + ", id_autor=" + libro.IDAutor + ", id_categoria=" + libro.IDCategoria + ", existencia=" + libro.Existencia + ", activo=" + libro.Activo);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool Modificar(Libro libro)
        {
            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "UPDATE libros SET isbn = @isbn, titulo = @titulo, " +
                               "id_autor = @id_autor, id_categoria = @id_categoria, " +
                               "existencia = @existencia, activo = @activo " +
                               "WHERE id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@isbn", libro.Titulo);
                comm.Parameters.AddWithValue("@titulo", libro.ISBN);
                comm.Parameters.AddWithValue("@id_autor", libro.IDAutor);
                comm.Parameters.AddWithValue("@id_categoria", libro.IDCategoria);
                comm.Parameters.AddWithValue("@existencia", libro.Existencia);
                comm.Parameters.AddWithValue("@activo", libro.Activo ? 1 : 0);
                comm.Parameters.AddWithValue("@id", libro.ID);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "id=" + libro.ID + ", isbn=" + libro.ISBN + ", titulo=" + libro.Titulo + ", id_autor=" + libro.IDAutor + ", id_categoria=" + libro.IDCategoria + ", existencia=" + libro.Existencia + ", activo=" + libro.Activo);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool ExisteISBN(string isbn)
        {
            bool existe = false;

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT id FROM libros WHERE isbn = @isbn";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@isbn", isbn);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    existe = true;
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "isbn=" + isbn);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return existe;
        }

        public bool DisminuirExistencia(int idLibro, int cantidad)
        {
            if (cantidad <= 0)
            {
                return false;
            }

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "UPDATE libros SET existencia = existencia - @cantidad " +
                               "WHERE id = @id AND existencia > @cantidad";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@cantidad", cantidad);
                comm.Parameters.AddWithValue("@id", idLibro);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "idLibro=" + idLibro + ", cantidad=" + cantidad);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool AumentarExistencia(int idLibro, int cantidad)
        {
            if (cantidad <= 0)
            {
                return false;
            }

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "UPDATE libros SET existencia = existencia + @cantidad " +
                               "WHERE id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@cantidad", cantidad);
                comm.Parameters.AddWithValue("@id", idLibro);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "idLibro=" + idLibro + ", cantidad=" + cantidad);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}