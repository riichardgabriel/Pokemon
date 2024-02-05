using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PokemonAPI
{
    public partial class Pokedex : Form
    {
        private const string ApiBaseUrl = "https://pokeapi.co/api/v2/pokemon/";

        public Pokedex()
        {
            InitializeComponent();
        }

        private async void btnConsultaPokemon_Click(object sender, EventArgs e)
        {
            string pokemonName = txtNomePokemon.Text.Trim();

            if (!string.IsNullOrEmpty(pokemonName))
            {
                try
                {
                    string apiUrl = $"{ApiBaseUrl}{pokemonName.ToLower()}/";
                    PokemonData pokemonData = await GetPokemonData(apiUrl);

                    if (pokemonData != null)
                    {
                        DisplayPokemonInfo(pokemonData);
                    }
                    else
                    {
                        MessageBox.Show("Pokémon não encontrado.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao consultar a API: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Digite o nome de um Pokémon.");
            }
        }

        private void DisplayPokemonInfo(PokemonData pokemonData)
        {
            // Exibir informações do Pokémon
            lbID.Text = $"ID: {pokemonData.id}";
            lbName.Text = $"Nome: {pokemonData.name}";
            lbTipo.Text = $"Tipo: {string.Join(", ", pokemonData.types.Select(t => t.type.name))}";
            lbPeso.Text = $"Peso: {pokemonData.weight} kg";
            lbAltura.Text = $"Altura: {pokemonData.height} m";

            // Exibir imagem do Pokémon
            DisplayPokemonImage(pokemonData.sprites.front_default);
        }

        private async Task<PokemonData> GetPokemonData(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    //instale atravez do NuGet Newtonsoft.Json
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<PokemonData>(jsonResponse);
                }
            }

            return null;
        }

        private void DisplayPokemonImage(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                pictureBox1.Size = new Size(300, 300);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Load(imageUrl);
            }
        }


        public class PokemonData
        {
            public string name { get; set; }
            public int id { get; set; }
            public List<TypeData> types { get; set; }
            public float weight { get; set; }
            public float height { get; set; }
            public Sprites sprites { get; set; }
        }

        public class TypeData
        {
            public Type type { get; set; }
        }

        public class Type
        {
            public string name { get; set; }
        }

        public class Sprites
        {
            public string front_default { get; set; }
        }
    }

}
