# Имплементација на игра - Брза географија
Проектна задача по Визуелно програмирање - Имплементација на играта Брза географија со можност за играње преку LAN и дополнителни квизови за тренинг
## 1.	Опис на проблемот
Нашата апликација ја имплементира играта Брза географија со можност за играње со двајца играчи преку локална мрежа. Дополнително, додадени се квизови и пробни игри за секоја од категории како дел за тренинг. 
<br><br>
Правилата за игра се следни: По случаен избор се избира една буква и за време од 5 минути, секој од играчите треба да напише по еден поим од следните категории: лично име, град, држава, река, планина, растение и животно. По завршување на рундата се доделуваат поени, така што доколку и двајцата играчи имаат валиден и различен поим за една категорија добиваат по 10 поени, доколку имаат валиден, но ист поим добиваат по 5 поени и доколку наведениот поим на некој од играчите е невалиден, истиот добива 0 поени за соодветната категорија. Играта завршува по желба на играчите, односно нема определен минимален број на рунди. Победник е оној кој има повеќе поени. Во нашата имплементација, валидноста на поимите за категориите град, држава, река и планина се проверуваат во база, додека останатите се оставени на чесност на играчите.
## 2.	Упатство за користење
По стартување на играта, корисникот го добива следниот приказ и има можност да избере една од двете опции, да започне нова игра со противник или да одбере некоја од игрите во тренинг делот. 
![image](https://github.com/darsov2/BrzaGeografija/assets/62809499/3ab4b333-acb0-4b1e-809f-d92bc9c31e24)
<br>Доколку се одбере опцијата „Започни игра“, се отвора нов прозорец (слика 2) каде корисникот може да избере да хостира или да се поврзе на веќе постоечка игра. За хостирање игра задолжително е внесувањето на порта, додека за поврзување на веќе постоечка игра, задолжителна е и IP адресата. Откако ќе се воспостави конекција, и на играчот-сервер и на играчот-клиент им се отвора нов прозорец (слика 3), каде во горниот дел е наведена буквата избрана по случаен избор и преостанатото време. 
<br>Кога некој од играчите смета дека е готов, со помош на копчето „КРАЈ“ може да ја заврши рундата, давајќи му на противникот завршни 10 секунди. Ова е означено со соодветна порака во долниот лев агол и намалување на преостанатото време. 
![image](https://github.com/darsov2/BrzaGeografija/assets/62809499/82a5cb6f-4f69-4bd2-889a-0786f70c4148)
<br>По истек на времето, се прикажуваат одговорите на корисникот заедно со доделените поени за тековната рунда (слика 4). Играчот-сервер има можност да започне нова рунда со клик на копчето „Нова игра“ во долниот десен агол, со што се генерира нова буква и играчите повторно го добиваат приказот од слика 3.  
<br>Доколку некој од играчите сака да ја заврши играта, истото може да го направи со клик на црвеното копче во горниот десен агол, при што времето се запира и се сметаат вкупните резултати, во кои тековната рунда не се зема предвид доколку не е заврешена. Вкупните резултати се прикажуваат во посебен прозорец кај двајцата играчи (слика 5).
<br>![image](https://github.com/darsov2/BrzaGeografija/assets/62809499/02569703-41e3-4856-842b-12c3f3c04ea6)
<br>Доколку од почетниот екран се одбере опцијата тренинг, корисникот може да избере помеѓу квизови од различни категории (слика 6) и игри каде треба да наведе што е можно повеќе поими на дадена буква (слика 7). 
<br>Концептот на квизовите е мошне интуитивен, се одбира еден одговор, по што, во зависност од тоа дали е точен се означува со зелено или црвено. На наредното прашање се преминува со клик на стрелката десно (слика 8). По излез од прозорецот, резултатите се прикажуваат во посебен прозорец налик слика 5.
<br>Игрите каде треба да се наведат што е можно повеќе поими на дадената буква функционираат така што за секој поим во базата се доделуваат по 3 секунди, а корисникот за даденото време треба да внесе што е можно повеќе во полето за одговор и да притисне Enter за валидација. Доколку поимот е валиден, ќе биде префрлен во листата долу (слика 9). По истек на времето се појавува прозорец со листа поими на буквата кои не биле внесени.
<br>![image](https://github.com/darsov2/BrzaGeografija/assets/62809499/ea96fa8e-3f24-4b99-930f-d0a476c6200d)
## 3.	Решение на проблемот
За имплементација на делот со квизови направена е една апстрактна класа Question со соодветни атрибути кои се заеднички за сите типови прашања. Од неа наследуваат класите MapQuestion, FlagQuestion, CapitalQuestion и LandmarkQuestion, каде дополнително се чува листа од објекти од соодветниот тип и истите имаат своја имплементација на методот getQuestionText. Секоја од овие класи во приватен метод генерира случаен распоред на понудените одговори.
<br>
```C#
    public abstract class Question
    {
        public int questionId { get; set; }
        public List<int> answers { get; set; }
        public int correctAnswer { get; set; }
        public string imageUrl { get; set; }
        public string answerQuestion(int ans)
        {
            if (ans == correctAnswer)
            {
                return "correct" + correctAnswer + ".png";
            }
            else
            {
                return "incorrect" + correctAnswer + ans + ".png";
            }
        }
        public abstract List<string> getAnswers();
        public abstract string getQuestionText();
        public abstract string getImageUrl();
    }
```
```C#
    public class CapitalQuestion : Question
    {
        static List<Country> countries;

        public CapitalQuestion(List<Country> _countries)
        {
            countries = _countries;
            questionId = FirebaseComm.getRandomNumber(countries.Count);
            answers = generateAnswers();
            imageUrl = countries.ElementAt(questionId).Png;
        }

        private List<int> generateAnswers()
        {
            List<int> ans = new List<int>();
            while(ans.Count != 4)
            {
                int rnd = FirebaseComm.getRandomNumber(countries.Count);
                if(ans.Contains(rnd) || rnd == questionId)
                {
                    continue;
                }
                ans.Add(rnd);
            }
            correctAnswer = FirebaseComm.getRandomNumber(4);
            ans[correctAnswer] = questionId;
            return ans;
        }
        public override List<string> getAnswers()
        {
            List<string> ans = new List<string>();
            answers.ForEach(a => ans.Add(countries.ElementAt(a).Capital));
            return ans;
        }

        public override string getQuestionText()
        {
            return "Кој е главен град на " + countries.ElementAt(questionId).Name;
        }

        public override string getImageUrl()
        {
            return countries.ElementAt(questionId).Png;
        }
    }
```
Објектите чии листи се чуваат во секоја од овие класи се еден вид модели за форматот на податоците кои се преземаат како JSON објект од Firebase. Истото е имплементирано преку статичка класа за комуникација со Firebase преку екстензијата FireSharp каде е конфигуриран клиент и посебни функции за секој објект.
<br>
```C#
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
```
```C#
    public class Country
    {
        public string Name { get; set; }
        public string Capital { get; set; }
        public string Png { get; set; }
    }
```
![image](https://github.com/darsov2/BrzaGeografija/assets/62809499/327903ce-e77d-4736-b072-350511aa703c)
<br>
За потребите на делот за проверка на одговорите од игрите, направена е посебна класа каде за секоја буква се чуваат листи од следните категории
<br>
```C#
    public class LetterData
    {
        public List<string> Countries { get; set; }
        public List<string> Cities { get; set; }
        public List<string> Rivers { get; set; }
        public List<string> Mountains { get; set; }
    }
```
Главниот, LAN, делот од играта е имплементиран така што и во клиентскиот и во серверскиот прозорец се чува по една инстанца од класата Game, што ја претставува тековната рунда, и соодветно листа од објекти од класата Game, за сите завршени рунди. Истата е означена како Serializable, за да може објектите да се серијализираат и како такви да се испратат преку мрежа.
<br>Методот CheckGame во рамки на класата Game служи за валидација на одговорите со противникот. Од Firebase се земаат податоците за буквата и се доделуваат поени според правилата. Во кодот е наведен еден пример услов за проверка на одговор во база и еден услов за проверка на различноста.
```C#
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
    }
```
```C#
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
        return points;
    }
```
Дополнително во двете форми, се чуваат инстанци од класите TcpClient и NetworkStream кои се потребни за воспоставување и одржување на конекцијата, како и испраќање на податоците. Поврзувањето започнува така што серверот отвора конекција која слуша на внесената порта и чека поврзување на клиенти. Кај клиентот се прави инстанца од IPEndPoint со внесената IP Адреса и порта и се прави обид за конекција. Откако конекцијта ќе биде воспоставена, двјцата играчи преминуваат на главниот екран.
```C#
        int port = Convert.ToInt32(textBox2.Text);
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();

        TcpClient client = server.AcceptTcpClient();
        NetworkStream stream = client.GetStream();

        ServerView serverView = new ServerView(server, client, stream, port);
        serverView.Show();
```
```C#
        int port = Convert.ToInt32(textBox2.Text);

        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), port);

        TcpClient client = new TcpClient();
        client.Connect(endpoint);

        NetworkStream stream = client.GetStream();

        ClientView clientView = new ClientView(client, stream);
        clientView.Show();
```
Постојаната комуникација меѓу сервер- и клиент-играчот е имплементирана на тој начин што во асинхрона функција двата хоста постојано чекаат пораки од противникот. Пораките се испраќаат на мрежен поток, бајт по бајт. По нивното пристигнување најпрвин се прави обид за десеријализација на објект од класата Game, што би претставувало крај на рундата, а доколку тој не е успешен се чита пораката и се презема бараната акција. Подолу е прикажан дел од кодот за оваа функционалност.
```C#
      private async Task ProcessClient(TcpClient client)
      {
          try
          {

              while (true)
              {
                  byte[] receivedData = new byte[client.ReceiveBufferSize];
                  int bytesRead = await stream.ReadAsync(receivedData, 0, client.ReceiveBufferSize);
                  string receivedObject = Encoding.UTF8.GetString(receivedData, 0, bytesRead);
                  try
                  {
                      Game deserializedObject = JsonConvert.DeserializeObject<Game>(receivedObject);
                      clientGame = deserializedObject;
                      clientDone = true;
                      timeLeft = Math.Min(timeLeft, 10);
                      if (serverDone)
                      {
                          fillOpponent(clientGame);
                          this.Height = 500;
                          label3.Hide();
                      }
                      label3.Show();
                  }
                  catch (Exception ex)
                  {
                      if (receivedObject.Equals("clientDone"))
                      {
                          label3.Show();
                          fillOpponent(clientGame);
                          this.Height = 500;
                      }
                      else if(receivedObject.Equals("resultsShown"))
                      { 
                          newGame = true;
                      }
                      else if(receivedObject.StartsWith("totalPoints"))
                      {
                          clientTotalPoints = Convert.ToInt32(receivedObject.Split(':')[1]);
                          clientTotal = true;
                          if (!stopGame)
                          {
                              button1_Click_1(null, null);
                          }
                          else
                          {
                              endGame();
                          }
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              MessageBox.Show("Проблем со процесирање на податоците од клиентска страна!\n\n" + ex.Message);
          }
          finally
          {
              tcpClient.Close();
              tcpClient = null;
          }
      }
```
Испраќањето на порака се врши со посебен метод, по потреба. Доколку се работи за објект, истиот се серијализира во JSON со користење на екстензијатa NewtonSoftJSON. Потоа, пораката се претвора во низа од бајти, која асинхроно се испраќа на мрежниот поток.
```C#
      private async void SendDataToClients(int type, string text, Game game = null)
      {
          string dataToSend = text;

          try
          {
              NetworkStream stream = tcpClient.GetStream();
              if(type == 0)
              {
                  byte[] data = Encoding.UTF8.GetBytes(dataToSend);
                  await stream.WriteAsync(data, 0, data.Length);
              }
              else
              {
                  string serializedObject = JsonConvert.SerializeObject(game);
                  byte[] data = Encoding.UTF8.GetBytes(serializedObject);
                  await stream.WriteAsync(data, 0, data.Length);
              }
          }
          catch...
      }
```
