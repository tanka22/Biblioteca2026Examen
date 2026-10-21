using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Biblioteca.Models;
using Biblioteca.Repositories;

namespace Biblioteca.Forms
{
    public partial class FrmLibros : Form
    {
        private int idSeleccionado = 0;

        public FrmLibros()
        {
            InitializeComponent();
        }

        private void FrmLibros_Load(object sender, EventArgs e)
        {
            CargarCombos();
            CargarTodos();
        }

        private void CargarCombos()
        {
            try
            {
                AutorRepositorio autorRepo = new AutorRepositorio();
                List<Autor> autores = autorRepo.ObtenerTodos();

                cmbAutor.DataSource = null;
                cmbAutor.DisplayMember = "NombreCompleto";
                cmbAutor.ValueMember = "ID";
                cmbAutor.DataSource = autores;

                CategoriaRepositorio categoriaRepo = new CategoriaRepositorio();
                List<Categoria> categorias = categoriaRepo.ObtenerTodos();

                cmbCategoria.DataSource = null;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "ID";
                cmbCategoria.DataSource = categorias;

                List<Categoria> filtro = new List<Categoria>();

                filtro.Add(new Categoria());
                filtro[0].ID = 0;
                filtro[0].Nombre = "Todas";

                for (int i = 0; i < categorias.Count; i++)
                {
                    filtro.Add(categorias[i]);
                }

                cmbFiltroCategoria.DataSource = null;
                cmbFiltroCategoria.DisplayMember = "Nombre";
                cmbFiltroCategoria.ValueMember = "ID";
                cmbFiltroCategoria.DataSource = filtro;
                cmbFiltroCategoria.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmLibros; Metodo=CargarCombos");
                MessageBox.Show("Error al cargar autores o categorías: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarTodos()
        {
            try
            {
                LibroRepositorio repo = new LibroRepositorio();
                List<Libro> lista = repo.ObtenerTodos();

                MostrarLista(lista);
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmLibros; Metodo=CargarTodos");
                MessageBox.Show("Error al cargar libros: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarLista(List<Libro> lista)
        {
            dgvLibros.DataSource = null;
            dgvLibros.DataSource = lista;
        }

        private void dgvLibros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Libro libro = dgvLibros.Rows[e.RowIndex].DataBoundItem as Libro;

            if (libro == null)
            {
                return;
            }

            idSeleccionado = libro.ID;
            txtISBN.Text = libro.ISBN;
            txtTitulo.Text = libro.Titulo;
            txtExistencia.Text = libro.Existencia.ToString();
            chkActivo.Checked = libro.Activo;
            cmbAutor.SelectedValue = libro.IDAutor;
            cmbCategoria.SelectedValue = libro.IDCategoria;
        }

        private void LimpiarCampos()
        {
            idSeleccionado = 0;
            txtISBN.Clear();
            txtTitulo.Clear();
            txtExistencia.Clear();
            chkActivo.Checked = true;
            cmbAutor.SelectedIndex = -1;
            cmbCategoria.SelectedIndex = -1;
            dgvLibros.ClearSelection();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string isbn = txtISBN.Text.Trim();

            if (isbn == "")
            {
                MessageBox.Show("El ISBN es obligatorio.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtTitulo.Text.Trim() == "")
            {
                MessageBox.Show("El título es obligatorio.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbAutor.SelectedValue == null || cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un autor y una categoría.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int existencia = Convert.ToInt32(txtExistencia.Text.Trim());

                if (existencia < -1)
                {
                    MessageBox.Show("La existencia no puede ser negativa.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LibroRepositorio repo = new LibroRepositorio();

                if (repo.ExisteISBN(isbn))
                {
                    MessageBox.Show("Ya existe un libro con ese ISBN.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Libro libro = new Libro();

                libro.ISBN = isbn;
                libro.Titulo = txtTitulo.Text.Trim();
                libro.IDAutor = Convert.ToInt32(cmbAutor.SelectedValue);
                libro.IDCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);
                //Así se va a poder convertir el texto escrito en un campo a un número entero.
                libro.Existencia = Convert.ToInt32(txtExistencia.Text); //Se corrigio esta linea de codigo
                libro.Activo = chkActivo.Checked;

                if (repo.Agregar(libro))
                {
                    MessageBox.Show("Libro registrado correctamente.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el libro.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmLibros; Metodo=btnAgregar_Click");
                MessageBox.Show("Error al registrar libro: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un libro de la lista.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string isbn = txtISBN.Text.Trim();

            if (isbn == "")
            {
                MessageBox.Show("El ISBN es obligatorio.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtTitulo.Text.Trim() == "")
            {
                MessageBox.Show("El título es obligatorio.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbAutor.SelectedValue == null || cmbCategoria.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un autor y una categoría.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int existencia = Convert.ToInt32(txtExistencia.Text.Trim());

                if (existencia < 0)
                {
                    MessageBox.Show("La existencia no puede ser negativa.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LibroRepositorio repo = new LibroRepositorio();
                List<Libro> encontrados = repo.BuscarPorISBN(isbn);

                if (encontrados.Count > 0 && encontrados[0].ID != idSeleccionado)
                {
                    MessageBox.Show("Ya existe otro libro con ese ISBN.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Libro libro = new Libro();

                libro.ID = idSeleccionado;
                libro.ISBN = isbn;
                libro.Titulo = txtTitulo.Text.Trim();
                libro.IDAutor = Convert.ToInt32(cmbAutor.SelectedValue);
                libro.IDCategoria = Convert.ToInt32(cmbCategoria.SelectedValue);
                libro.Existencia = existencia;
                libro.Activo = chkActivo.Checked;

                if (repo.Modificar(libro))
                {
                    MessageBox.Show("Libro modificado correctamente.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarTodos();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el libro.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmLibros; Metodo=btnModificar_Click");
                MessageBox.Show("Error al modificar libro: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarTitulo_Click(object sender, EventArgs e)
        {
            string titulo = txtBuscarTitulo.Text.Trim();

            if (titulo == "")
            {
                MessageBox.Show("Escriba un título para buscar.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                LibroRepositorio repo = new LibroRepositorio();
                List<Libro> lista = repo.BuscarPorISBN(titulo);

                MostrarLista(lista);

                if (lista.Count == 0)
                {
                    MessageBox.Show("No se encontraron libros con ese título.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmLibros; Metodo=btnBuscarTitulo_Click");
                MessageBox.Show("Error al buscar libro: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscarISBN_Click(object sender, EventArgs e)
        {
            string isbn = txtBuscarISBN.Text.Trim();

            if (isbn == "")
            {
                MessageBox.Show("Escriba un ISBN para buscar.",
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                LibroRepositorio repo = new LibroRepositorio();
                List<Libro> lista = repo.BuscarPorISBN(isbn);

                MostrarLista(lista);

                if (lista.Count == 0)
                {
                    MessageBox.Show("No se encontraron libros con ese ISBN.",
                        "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmLibros; Metodo=btnBuscarISBN_Click");
                MessageBox.Show("Error al buscar libro: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                LibroRepositorio repo = new LibroRepositorio();
                int idCategoria = Convert.ToInt32(cmbFiltroCategoria.SelectedValue);

                if (idCategoria == 0)
                {
                    CargarTodos();
                }
                else
                { //se hizo unos cambios en esta parte del codigo para que filtre por categoria correctamente
                    List<Libro> lista = repo.FiltrarPorCategoria(Convert.ToInt32(cmbFiltroCategoria.SelectedValue));
                    MostrarLista(lista);
                }
            }
            catch (Exception ex)
            {
                new ErrorRepositorio().Registrar(ex, "Formulario=FrmLibros; Metodo=btnFiltrar_Click");
                MessageBox.Show("Error al filtrar libros: " + ex.Message,
                    "Biblioteca", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            if (cmbFiltroCategoria.Items.Count > 0)
            {
                cmbFiltroCategoria.SelectedIndex = 0;
            }

            CargarTodos();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}