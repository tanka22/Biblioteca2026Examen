using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Biblioteca.Models;
using Biblioteca.Repositories;

namespace Biblioteca.Forms
{
    public partial class FrmAutores : Form
    {
        private int idSeleccionado = 0;

        public FrmAutores()
        {
            InitializeComponent();
        }

        private void FrmAutores_Load(object sender, EventArgs e)
        {
            CargarTodos();
        }

        private void CargarTodos()
        {
            try
            {
                AutorRepositorio repo = new AutorRepositorio();
                List<Autor> lista = repo.ObtenerTodos();

                dgvAutores.DataSource = null;
                dgvAutores.DataSource = lista;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAutores; Metodo=CargarTodos");
                MessageBox.Show("Error al cargar autores: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAutores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Autor autor = dgvAutores.Rows[e.RowIndex].DataBoundItem as Autor;

            if (autor == null)
            {
                return;
            }

            idSeleccionado = autor.ID;
            txtNombre.Text = autor.Nombre;
            txtApellidos.Text = autor.Apellidos;
            txtNacionalidad.Text = autor.Nacionalidad;
        }

        private void LimpiarCampos()
        {
            idSeleccionado = 0;
            txtNombre.Clear();
            txtApellidos.Clear();
            txtNacionalidad.Clear();
            dgvAutores.ClearSelection();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("El nombre es obligatorio.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Autor autor = new Autor();

                autor.Nombre = txtNombre.Text.Trim();
                autor.Apellidos = txtApellidos.Text.Trim();
                autor.Nacionalidad = txtNacionalidad.Text.Trim();

                AutorRepositorio repo = new AutorRepositorio();

                //se agrego otro if para validar que no se repita el autor
                if (repo.Existe(autor.Nombre, autor.Apellidos))
                {
                    MessageBox.Show("El autor ya existe.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (repo.Agregar(autor))
                {
                    MessageBox.Show("Autor registrado correctamente.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el autor.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAutores; Metodo=btnAgregar_Click");
                MessageBox.Show("Error al registrar autor: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un autor de la lista.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("El nombre es obligatorio.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Autor autor = new Autor();

                autor.ID = idSeleccionado;
                autor.Nombre = txtNombre.Text.Trim();
                autor.Apellidos = txtApellidos.Text.Trim();
                autor.Nacionalidad = txtNacionalidad.Text.Trim();

                AutorRepositorio repo = new AutorRepositorio();

                if (repo.Modificar(autor))
                {
                    MessageBox.Show("Autor modificado correctamente.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el autor.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmAutores; Metodo=btnModificar_Click");
                MessageBox.Show("Error al modificar autor: " + ex.Message,
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