using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    [Serializable]
    public class Game
    {
        public char Letter { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string River { get; set; }
        public string Mountain { get; set; }
        public string Plant { get; set; }
        public string Animal { get; set; }
        public string Thing { get; set; }
        public int Points { get; set; }

        public Game(string name, string city, string country, string river, string mountain, string plant, string animal, string thing, int points)
        {
            Name = name;
            City = city;
            Country = country;
            River = river;
            Mountain = mountain;
            Plant = plant;
            Animal = animal;
            Thing = thing;
            Points = points;
        }

        [JsonConstructor]
        public Game(char letter, string name, string city, string country, string river, string mountain, string plant, string animal, string thing, int points) : this(letter)
        {
            Name = name;
            City = city;
            Country = country;
            River = river;
            Mountain = mountain;
            Plant = plant;
            Animal = animal;
            Thing = thing;
            Points = points;
        }

        public Game(char letter)
        {
            Letter = letter;
        }

        public int[] CheckGame(Game opponentGame)
        {
            LetterData letterData = FirebaseComm.FetchLetterData(Letter);
            int[] points = { 0, 0, 0, 0, 0, 0, 0, 0 };
            if (letterData.Cities != null && letterData.Cities.Any(x => x.Equals(City, StringComparison.OrdinalIgnoreCase)))
            {
                points[1] = 10;
                if (letterData.Cities.Any(x => x.Equals(City, StringComparison.OrdinalIgnoreCase)) && opponentGame.City.Equals(City, StringComparison.OrdinalIgnoreCase))
                {
                    points[1] -= 5;
                }
            }
            if (letterData.Countries != null && letterData.Countries.Any(x => x.Equals(Country, StringComparison.OrdinalIgnoreCase)))
            {
                points[2] = 10;
                if (letterData.Countries.Any(x => x.Equals(Country, StringComparison.OrdinalIgnoreCase)) && opponentGame.Country.Equals(Country, StringComparison.OrdinalIgnoreCase))
                {
                    points[2] -= 5;
                }
            }
            if (letterData.Rivers != null && letterData.Rivers.Any(x => x.Equals(River, StringComparison.OrdinalIgnoreCase)))
            {
                points[3] = 10;
                if (letterData.Rivers.Any(x => x.Equals(River, StringComparison.OrdinalIgnoreCase)) && opponentGame.River.Equals(River, StringComparison.OrdinalIgnoreCase))
                {
                    points[3] -= 5;
                }
            }
            if (letterData.Mountains != null && letterData.Mountains.Any(x => x.Equals(Mountain, StringComparison.OrdinalIgnoreCase)))
            {
                points[4] = 10;
                if (letterData.Mountains.Any(x => x.Equals(Mountain, StringComparison.OrdinalIgnoreCase)) && opponentGame.Mountain.Equals(Mountain, StringComparison.OrdinalIgnoreCase))
                {
                    points[4] -= 5;
                }
            }

            if(Name.StartsWith(Letter.ToString()))
            {
                points[0] = 10;
                if (opponentGame.Name.StartsWith(Letter.ToString()) && opponentGame.Name.Equals(Name, StringComparison.OrdinalIgnoreCase))
                {
                    points[0] -= 5;
                }
            }
            if (Plant.StartsWith(Letter.ToString()))
            {
                points[5] = 10;
                if (opponentGame.Plant.StartsWith(Letter.ToString()) && opponentGame.Plant.Equals(Plant, StringComparison.OrdinalIgnoreCase))
                {
                    points[5] -= 5;
                }
            }
            if (Animal.StartsWith(Letter.ToString()))
            {
                points[6] = 10;
                if (opponentGame.Animal.StartsWith(Letter.ToString()) && opponentGame.Animal.Equals(Animal, StringComparison.OrdinalIgnoreCase))
                {
                    points[6] -= 5;
                }
            }
            if (Thing.StartsWith(Letter.ToString()))
            {
                points[7] = 10;
                if (opponentGame.Thing.StartsWith(Letter.ToString()) && opponentGame.Thing.Equals(Thing, StringComparison.OrdinalIgnoreCase))
                {
                    points[7] -= 5;
                }
            }

            return points;
        }
    }
}
