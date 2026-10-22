using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Biblioteca.Interfaces;
using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public class PrestamoRepositorio : IPrestamo
    {
        public List<Prestamo> ObtenerTodos()
        {
            List<Prestamo> lista = new List<Prestamo>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT p.id, p.id_alumno, p.fecha_prestamo, p.fecha_limite, p.estado, " +
                               "a.matricula, CONCAT(a.nombre, ' ', a.apellidos) AS alumno " +
                               "FROM prestamos p " +
                               "INNER JOIN alumnos a ON p.id_alumno = a.id " +
                               "ORDER BY p.id DESC";

                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Prestamo prestamo = new Prestamo();

                        prestamo.ID = Convert.ToInt32(dr["id"].ToString());
                        prestamo.IDAlumno = Convert.ToInt32(dr["id_alumno"].ToString());
                        prestamo.FechaPrestamo = Convert.ToDateTime(dr["fecha_prestamo"].ToString());
                        prestamo.FechaLimite = Convert.ToDateTime(dr["fecha_limite"].ToString());
                        prestamo.Estado = dr["estado"].ToString();
                        prestamo.Matricula = dr["matricula"].ToString();
                        prestamo.NombreAlumno = dr["alumno"].ToString();

                        lista.Add(prestamo);
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

        public List<Prestamo> ObtenerActivos()
        {
            List<Prestamo> lista = new List<Prestamo>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT p.id, p.id_alumno, p.fecha_prestamo, p.fecha_limite, p.estado, " +
                               "a.matricula, CONCAT(a.nombre, ' ', a.apellidos) AS alumno " +
                               "FROM prestamos p " +
                               "INNER JOIN alumnos a ON p.id_alumno = a.id " +
                               "WHERE p.estado = 'Devuelto' " +
                               "ORDER BY p.id DESC";

                MySqlCommand comm = new MySqlCommand(query, conn);
                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Prestamo prestamo = new Prestamo();

                        prestamo.ID = Convert.ToInt32(dr["id"].ToString());
                        prestamo.IDAlumno = Convert.ToInt32(dr["id_alumno"].ToString());
                        prestamo.FechaPrestamo = Convert.ToDateTime(dr["fecha_prestamo"].ToString());
                        prestamo.FechaLimite = Convert.ToDateTime(dr["fecha_prestamo"].ToString());
                        prestamo.Estado = dr["estado"].ToString();
                        prestamo.Matricula = dr["matricula"].ToString();
                        prestamo.NombreAlumno = dr["alumno"].ToString();

                        lista.Add(prestamo);
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

        public Prestamo Buscar(int id)
        {
            Prestamo prestamo = null;

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT p.id, p.id_alumno, p.fecha_prestamo, p.fecha_limite, p.estado, " +
                               "a.matricula, CONCAT(a.nombre, ' ', a.apellidos) AS alumno " +
                               "FROM prestamos p " +
                               "INNER JOIN alumnos a ON p.id_alumno = a.id " +
                               "WHERE p.id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@id", id);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.Read())
                {
                    prestamo = new Prestamo();

                    prestamo.ID = Convert.ToInt32(dr["id"].ToString());
                    prestamo.IDAlumno = Convert.ToInt32(dr["id_alumno"].ToString());
                    prestamo.FechaPrestamo = Convert.ToDateTime(dr["fecha_prestamo"].ToString());
                    prestamo.FechaLimite = Convert.ToDateTime(dr["fecha_limite"].ToString());
                    prestamo.Estado = dr["estado"].ToString();
                    prestamo.Matricula = dr["matricula"].ToString();
                    prestamo.NombreAlumno = dr["alumno"].ToString();
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

            return prestamo;
        }

        public List<DetallePrestamo> ObtenerDetalles(int idPrestamo)
        {
            List<DetallePrestamo> lista = new List<DetallePrestamo>();

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "SELECT d.id, d.id_prestamo, d.id_libro, d.cantidad, l.titulo " +
                               "FROM detalle_prestamo d " +
                               "INNER JOIN libros l ON d.id_libro = l.id " +
                               "WHERE d.id_prestamo = @id_prestamo ORDER BY d.id";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@id_prestamo", idPrestamo);

                MySqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DetallePrestamo detalle = new DetallePrestamo();

                        detalle.ID = Convert.ToInt32(dr["id"].ToString());
                        detalle.IDPrestamo = Convert.ToInt32(dr["id_prestamo"].ToString());
                        detalle.IDLibro = Convert.ToInt32(dr["id_libro"].ToString());
                        detalle.Cantidad = Convert.ToInt32(dr["cantidad"].ToString());
                        detalle.Titulo = dr["titulo"].ToString();

                        lista.Add(detalle);
                    }
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "idPrestamo=" + idPrestamo);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return lista;
        }

        public bool Registrar(Prestamo prestamo, List<DetallePrestamo> detalles)
        {
            if (prestamo == null || detalles == null || detalles.Count == 0)
            {
                return false;
            }

            AlumnoRepositorio alumnoRepo = new AlumnoRepositorio();
            Alumno alumno = alumnoRepo.Buscar(prestamo.IDAlumno);

            if (alumno == null)
            {
                return false;
            }

            LibroRepositorio libroRepo = new LibroRepositorio();

            for (int i = 0; i < detalles.Count; i++)
            {
                DetallePrestamo detalle = detalles[i];

                if (detalle == null || detalle.Cantidad < 0)
                {
                    return false;
                }

                Libro libro = libroRepo.Buscar(detalle.IDLibro);

                if (libro == null)
                {
                    return false;
                }

                if (libro.Existencia < detalle.Cantidad)
                {
                    return false;
                }
            }

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "INSERT INTO prestamos(id_alumno, fecha_prestamo, fecha_limite, estado) " +
                               "VALUES(@id_alumno, @fecha_prestamo, @fecha_limite, 'Activo')";

                MySqlCommand comm = new MySqlCommand(query, conn);

                comm.Parameters.AddWithValue("@id_alumno", prestamo.IDAlumno);
                comm.Parameters.AddWithValue("@fecha_prestamo", prestamo.FechaPrestamo.Date);
                comm.Parameters.AddWithValue("@fecha_limite", prestamo.FechaLimite.Date);

                int filas = comm.ExecuteNonQuery();

                if (filas == 0)
                {
                    return false;
                }

                string queryID = "SELECT LAST_INSERT_ID()";

                MySqlCommand commID = new MySqlCommand(queryID, conn);
                int idPrestamo = Convert.ToInt32(commID.ExecuteScalar().ToString());

                for (int i = 0; i < detalles.Count; i++)
                {
                    DetallePrestamo detalle = detalles[i];

                    string queryDetalle = "INSERT INTO detalle_prestamo(id_prestamo, id_libro, cantidad) " +
                                          "VALUES(@id_prestamo, @id_libro, @cantidad)";

                    MySqlCommand commDetalle = new MySqlCommand(queryDetalle, conn);

                    commDetalle.Parameters.AddWithValue("@id_prestamo", idPrestamo);
                    commDetalle.Parameters.AddWithValue("@id_libro", detalle.IDLibro);
                    commDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);

                    commDetalle.ExecuteNonQuery();

                    libroRepo.DisminuirExistencia(detalle.IDLibro, 1);

                    detalle.IDPrestamo = idPrestamo;
                }

                prestamo.ID = idPrestamo;

                return true;
            }
            catch (Exception ex)
            {
                string parametros = "id_alumno=" + prestamo.IDAlumno + ", fecha_prestamo=" + prestamo.FechaPrestamo.Date + ", fecha_limite=" + prestamo.FechaLimite.Date + ", detalles_count=" + detalles.Count;
                new ErrorRepositorio().Registrar(ex, parametros);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool RegistrarDevolucion(int idPrestamo)
        {
            if (idPrestamo <= 0)
            {
                return false;
            }

            Prestamo prestamo = Buscar(idPrestamo);

            if (prestamo == null)
            {
                return false;
            }

            List<DetallePrestamo> detalles = ObtenerDetalles(idPrestamo);

            if (detalles.Count == 0)
            {
                return false;
            }

            LibroRepositorio libroRepo = new LibroRepositorio();

            for (int i = 0; i < detalles.Count; i++)
            {
                DetallePrestamo detalle = detalles[i];

                libroRepo.AumentarExistencia(detalle.IDLibro, 1);
            }

            MySqlConnection conn = new MySqlConnection(Utils.connStr);
            try
            {
                conn.Open();

                string query = "UPDATE prestamos SET estado = 'Devuelto' WHERE id = @id";

                MySqlCommand comm = new MySqlCommand(query, conn);
                comm.Parameters.AddWithValue("@id", idPrestamo);

                int filas = comm.ExecuteNonQuery();

                return filas > 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "idPrestamo=" + idPrestamo);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}