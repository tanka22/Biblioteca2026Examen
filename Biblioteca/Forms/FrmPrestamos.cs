using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Biblioteca.Interfaces;
using Biblioteca.Models;
using Biblioteca.Repositories;

namespace Biblioteca.Forms
{
    public partial class FrmPrestamos : Form
    {
        public FrmPrestamos()
        {
            InitializeComponent();

            dtpFechaPrestamo.Value = DateTime.Today;
            dtpFechaLimite.Value = DateTime.Today.AddDays(7);
        }

        private void FrmPrestamos_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                CargarAlumnos();
                CargarLibros();
                CargarPrestamos();
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmPrestamos; Metodo=CargarDatos");
                MessageBox.Show("Error al cargar los datos: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarAlumnos()
        {
            AlumnoRepositorio repo = new AlumnoRepositorio();
            List<Alumno> alumnos = repo.ObtenerTodos();

            cmbAlumno.DataSource = null;
            cmbAlumno.DisplayMember = "NombreCompleto";
            cmbAlumno.ValueMember = "ID";
            cmbAlumno.DataSource = alumnos;
        }

        private void CargarLibros()
        {
            LibroRepositorio repo = new LibroRepositorio();
            List<Libro> libros = repo.ObtenerTodos();

            cmbLibro.DataSource = null;
            cmbLibro.DisplayMember = "Titulo";
            cmbLibro.ValueMember = "Titulo";
            cmbLibro.DataSource = libros;
        }

        private void CargarPrestamos()
        {
            PrestamoRepositorio repo = new PrestamoRepositorio();
            List<Prestamo> prestamos = repo.ObtenerActivos();

            dgvPrestamos.DataSource = null;
            dgvPrestamos.DataSource = prestamos;

            dgvDetalles.DataSource = null;
        }

        private void dgvPrestamos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPrestamos.CurrentRow == null)
            {
                return;
            }

            Prestamo prestamo = dgvPrestamos.CurrentRow.DataBoundItem as Prestamo;

            if (prestamo == null)
            {
                return;
            }

            try
            {
                PrestamoRepositorio repo = new PrestamoRepositorio();
                List<DetallePrestamo> detalles = repo.ObtenerDetalles(prestamo.ID);

                dgvDetalles.DataSource = null;
                dgvDetalles.DataSource = detalles;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmPrestamos; Metodo=dgvPrestamos_SelectionChanged");
                MessageBox.Show("Error al cargar el detalle: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbAlumno.SelectedValue == null || cmbLibro.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un alumno y un libro.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtCantidad.Text.Trim() == "")
            {
                MessageBox.Show("Escriba la cantidad de libros a prestar.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            { //se corrigio este linea, asi ya se puede convertir a enteros y validar que sea positivo
                if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("La cantidad debe ser un número entero positivo.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Prestamo prestamo = new Prestamo();

                prestamo.IDAlumno = Convert.ToInt32(cmbAlumno.SelectedValue);
                //se cambio a value asi se asigna un datetime en vez de una cadena de texto
                prestamo.FechaPrestamo = dtpFechaPrestamo.Value;
                prestamo.FechaLimite = dtpFechaLimite.Value.Date;
                prestamo.Estado = "Activo";

                DetallePrestamo detalle = new DetallePrestamo();

                detalle.IDLibro = Convert.ToInt32(cmbLibro.SelectedValue);
                detalle.Cantidad = cantidad;

                List<DetallePrestamo> detalles = new List<DetallePrestamo>();
                detalles.Add(detalle);

                IPrestamo repo = new PrestamoRepositorio();

                //se cambio el metodo Registrar para que devuelva un booleano y asi saber si se registro correctamente o no
                if (repo.Registrar(prestamo,detalles))
                {
                    MessageBox.Show("Préstamo registrado correctamente.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtCantidad.Clear();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el préstamo. " +
                        "Verifique la cantidad, la existencia, el estado del alumno, " +
                        "el estado del libro y las fechas.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmPrestamos; Metodo=btnRegistrar_Click");
                MessageBox.Show("Error al registrar préstamo: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtCantidad.Clear();
            dtpFechaPrestamo.Value = DateTime.Today;
            dtpFechaLimite.Value = DateTime.Today.AddDays(7);
            dgvPrestamos.ClearSelection();
            dgvDetalles.DataSource = null;
        }
    }
}