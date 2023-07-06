using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrzaGeografija.Classes
{
    public static class FirebaseComm
    {
        private static Random random = new Random();

        private static IFirebaseConfig firebaseConfig = new FirebaseConfig
        {
            AuthSecret = "BLQ8VsdNqhu6Y3WrWRNoVA6LKvFBFYBcqUTo4XSt",
            BasePath = "https://geo-kviz-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        private static IFirebaseClient firebaseClient = new FireSharp.FirebaseClient(firebaseConfig);

        public static List<Country> FetchCountries()
        {
            if (firebaseClient == null)
            {
                MessageBox.Show("Проблем со поврзување со базата на податоци");
                return new List<Country>();
            }
            else
            {
                FirebaseResponse response = firebaseClient.Get("countries");
                return response.ResultAs<List<Country>>();
            }
        }

        public static List<Landmark> FetchLandmarks()
        {
            if (firebaseClient == null)
            {
                MessageBox.Show("Проблем со поврзување со базата на податоци");
                return new List<Landmark>();
            }
            else
            {
                FirebaseResponse response = firebaseClient.Get("landmarks");
                return response.ResultAs<List<Landmark>>();
            }
        }

        public static LetterData FetchLetterData(char letter)
        {
            if (firebaseClient == null)
            {
                MessageBox.Show("Проблем со поврзување со базата на податоци");
                return new LetterData();
            }
            else
            {
                FirebaseResponse response = firebaseClient.Get("letter_data/" + letter);
                return response.ResultAs<LetterData>();
            }
        }

        public static List<string> FetchCities(char letter)
        {
            if (firebaseClient == null)
            {
                MessageBox.Show("Проблем со поврзување со базата на податоци");
                return new List<string>();
            }
            else
            {
                FirebaseResponse response = firebaseClient.Get("letter_data/" + letter + "/cities");
                return response.ResultAs<List<string>>();
            }
        }

        public static int getRandomNumber(int max)
        {
            return random.Next(max);
        }
    }
}
