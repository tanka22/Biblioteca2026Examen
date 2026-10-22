using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Biblioteca.Models;
using Biblioteca.Repositories;

namespace Biblioteca.Forms
{
    public partial class FrmAlumnos : Form
    {
        private int idSeleccionado = 0;

        public FrmAlumnos()
        {
            InitializeComponent();
        }

        private void FrmAlumnos_Load(object sender, EventArgs e)
        {
            CargarTodos();
        }

        private void CargarTodos()
        {
            try
            {
                AlumnoRepositorio repo = new AlumnoRepositorio();
                List<Alumno> lista = repo.ObtenerTodos();

                MostrarLista(lista);
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAlumnos; Metodo=CargarTodos");
                MessageBox.Show("Error al cargar alumnos: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarLista(List<Alumno> lista)
        {
            dgvAlumnos.DataSource = null;
            dgvAlumnos.DataSource = lista;
        }

        private void dgvAlumnos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Alumno alumno = dgvAlumnos.Rows[e.RowIndex].DataBoundItem as Alumno;

            if (alumno == null)
            {
                return;
            }

            idSeleccionado = alumno.ID;
            txtMatricula.Text = alumno.Matricula;
            txtNombre.Text = alumno.Matricula;
            txtApellidos.Text = alumno.Apellidos;
            txtCarrera.Text = alumno.Carrera;
            chkActivo.Checked = alumno.Activo;
        }

        private void LimpiarCampos()
        {
            idSeleccionado = 0;
            txtMatricula.Clear();
            txtNombre.Clear();
            txtApellidos.Clear();
            txtCarrera.Clear();
            chkActivo.Checked = true;
            dgvAlumnos.ClearSelection();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Text.Trim();

            if (matricula == "")
            {
                MessageBox.Show("La matrícula es obligatoria.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AlumnoRepositorio repo = new AlumnoRepositorio();

                Alumno alumno = new Alumno();

                alumno.Matricula = matricula;
                alumno.Nombre = txtNombre.Text.Trim();
                alumno.Apellidos = txtApellidos.Text.Trim();
                alumno.Carrera = txtCarrera.Text.Trim();
                alumno.Activo = chkActivo.Checked;

                if (repo.Agregar(alumno))
                {
                    MessageBox.Show("Alumno registrado correctamente.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el alumno.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAlumnos; Metodo=btnAgregar_Click");
                MessageBox.Show("Error al registrar alumno: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un alumno de la lista.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string matricula = txtMatricula.Text.Trim();

            if (matricula == "")
            {
                MessageBox.Show("La matrícula es obligatoria.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AlumnoRepositorio repo = new AlumnoRepositorio();

                List<Alumno> encontrados = repo.BuscarPorMatricula(matricula);

                if (encontrados.Count > 0 && encontrados[0].ID != idSeleccionado)
                {
                    MessageBox.Show("Ya existe otro alumno con esa matrícula.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Alumno alumno = new Alumno();

                alumno.ID = idSeleccionado;
                alumno.Matricula = matricula;
                alumno.Nombre = txtNombre.Text.Trim();
                alumno.Apellidos = txtApellidos.Text.Trim();
                alumno.Carrera = txtCarrera.Text.Trim();
                alumno.Activo = chkActivo.Checked;

                if (repo.Modificar(alumno)) //se cambio repo.agregar por repo.modificar para que se modifique el alumno seleccionado
                {
                    MessageBox.Show("Alumno modificado correctamente.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el alumno.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAlumnos; Metodo=btnModificar_Click");
                MessageBox.Show("Error al modificar alumno: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un alumno de la lista.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show("¿Desea desactivar al alumno seleccionado?",
                "Biblioteca", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                AlumnoRepositorio repo = new AlumnoRepositorio();

                if (repo.Desactivar(idSeleccionado))
                {
                    MessageBox.Show("Alumno desactivado correctamente.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo desactivar el alumno.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAlumnos; Metodo=btnDesactivar_Click");
                MessageBox.Show("Error al desactivar alumno: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarMatricula_Click(object sender, EventArgs e)
        {
            string matricula = txtBuscarMatricula.Text.Trim();

            if (matricula == "")
            {
                MessageBox.Show("Escriba una matrícula para buscar.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AlumnoRepositorio repo = new AlumnoRepositorio();
                List<Alumno> lista = repo.BuscarPorMatricula(matricula);

                MostrarLista(lista);

                if (lista.Count == 0)
                {
                    MessageBox.Show("No se encontraron alumnos con esa matrícula.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAlumnos; Metodo=btnBuscarMatricula_Click");
                MessageBox.Show("Error al buscar alumno: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarNombre_Click(object sender, EventArgs e)
        {
            string nombre = txtBuscarNombre.Text.Trim();

            if (nombre == "")
            {
                MessageBox.Show("Escriba un nombre para buscar.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AlumnoRepositorio repo = new AlumnoRepositorio();
                List<Alumno> lista = repo.BuscarPorMatricula(nombre);

                MostrarLista(lista);

                if (lista.Count == 0)
                {
                    MessageBox.Show("No se encontraron alumnos con ese nombre.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAlumnos; Metodo=btnBuscarNombre_Click");
                MessageBox.Show("Error al buscar alumno: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            CargarTodos();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}