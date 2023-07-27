using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;

namespace C_em_Firebase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FirebaseClient cliente = new FirebaseClient("Coloque sua url", new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult("Coloque seu Token")
        });
        public async void obtendoDados()
        {
            lboItens.Items.Clear();
            var resultado = await cliente.Child("produtos")
                .OnceAsJsonAsync();
            JsonDocument importandoMinhaBase = JsonDocument.Parse(resultado);
            JsonElement filho = importandoMinhaBase.RootElement;

            if (filho.ValueKind.ToString() == "Null") return;

            foreach(var item in filho.EnumerateObject())
            {
                JsonElement produtoFirebase = item.Value;
                string nome = produtoFirebase.GetProperty("Nome").GetString()!;
                string desc = produtoFirebase.GetProperty("Desc").GetString()!;
                double preco = produtoFirebase.GetProperty("Preco").GetDouble();
                string id = item.Name;
                Produtos produto = new Produtos(id, nome, desc, preco);
                lboItens.Items.Add(produto);             
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            obtendoDados();
        }

        private void lboItens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lboItens.SelectedItem == null) return;
            Produtos produto = (lboItens.SelectedItem as Produtos)!;
            txtNome.Text = produto.Nome;
            txtDesc.Text = produto.Desc;
            txtPreco.Text = produto.Preco.ToString();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNome.Text;
            string desc = txtDesc.Text;
            double preco = double.Parse(txtPreco.Text);
            Produtos produto = new Produtos("", nome, desc, preco);
            var json = JsonConvert.SerializeObject(produto);
            await cliente.Child("produtos").PostAsync(json);
            obtendoDados();
        }

        private async void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            if (lboItens.SelectedItem == null) return;
            Produtos produto = (lboItens.SelectedItem as Produtos)!;
            await cliente.Child("produtos").Child(produto.id).DeleteAsync();
            obtendoDados();
        }

        private async void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (lboItens.SelectedItem == null) return;
            Produtos produto = (lboItens.SelectedItem as Produtos)!;
            string nome = txtNome.Text;
            string desc = txtDesc.Text;
            double preco = double.Parse(txtPreco.Text);
            Produtos atualizado = new Produtos(produto.id, nome, desc, preco);
            await cliente.Child("produtos").Child(produto.id).PutAsync(atualizado);
            obtendoDados();
        }
    }
}
