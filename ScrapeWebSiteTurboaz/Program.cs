using IronWebScraper;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using MySql.Data.MySqlClient;

namespace ScrapeWebSiteTurboaz
{
    internal class Program
    {
        static object x = new object();

        static void Main(string[] args)
        {
            var mylink = new List<string>();
            mylink.Add("https://turbo.az/autos");
            for (int i = 2; i < 25; i++) mylink.Add($"https://turbo.az/autos?page={i}");

            for (int i = 0; i < mylink.Count; i++)
            {
                var temp = new SiteScraper();
                temp.MaxHttpConnectionLimit = 10;
                temp.URL = mylink[i];
                temp.Start();

                temp.Stop();
                for (int j = 0; j < temp.Links.Count; j++)
                {
                    SiteScraper2 temp2 = new SiteScraper2();
                    temp2.MaxHttpConnectionLimit = 10;
                    temp2.URL = temp.Links[j];
                    temp2.Start();
                    temp2.Stop();
                }
            }
        }
    }

    class SiteScraper2 : WebScraper
    {
        static object x = new object();
        public string URL { get; set; }
        public override void Init()
        {
            this.LoggingLevel = LogLevel.All;

            foreach (var ua in CommonUserAgents.All)
            {
                Identities.Add(new HttpIdentity
                {
                    UserAgent = ua,
                    UseCookies = true
                });
            }
            this.Request(URL, Parse);
        }
        public void JsonlToJson()
        {
            // var TextJson = JsonSerializer.Serialize(MyBooks, new JsonSerializerOptions() { WriteIndented = true });
            // File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Books.json", TextJson);

        }
        public List<string> Links { get; set; } = new List<string>();
        public override void Parse(Response response)
        {


            try
            {
                int number, num2;
                bool IsSalon = true;
                try
                {
                    var temp5 = response.Css(".page-content > div")[0].ChildNodes[3].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText;
                    number = 3;
                    num2 = 1;
                    IsSalon = true;
                }
                catch (Exception)
                {
                    number = 4;
                    num2 = 0;
                    IsSalon = false;
                }
                string description, saticiAdi, Seher, Marka, model, Il, BanNovu, Reng, Muherrik, MuherrikGucu, Yanacaq, Yurus, Karopka, Oturucu, Teze, Qiymet;
                List<string> ImgUrls = new List<string>();
                var Contacts = new List<string>();
                var Extras = new List<string>();
                string IdTittle = URL.Split('/')[URL.Split('/').Length - 1];
                if (IsSalon)
                {
                    Seher = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText;
                    Marka = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[1].InnerText;
                    model = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[1].InnerText;
                    Il = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[3].ChildNodes[1].InnerText;
                    BanNovu = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[4].ChildNodes[1].InnerText;
                    Reng = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[5].ChildNodes[1].InnerText;
                    Muherrik = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[6].ChildNodes[1].InnerText;
                    MuherrikGucu = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[7].ChildNodes[1].InnerText;
                    Yanacaq = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[8].ChildNodes[1].InnerText;
                    Yurus = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[9].ChildNodes[1].InnerText;
                    Karopka = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[10].ChildNodes[1].InnerText;
                    Oturucu = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[11].ChildNodes[1].InnerText;
                    Teze = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[12].ChildNodes[1].InnerText;
                    Qiymet = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[0].ChildNodes[0].ChildNodes[13].ChildNodes[1].InnerText;
                    var ImgObj = response.Css(".page-content > div")[0].ChildNodes[1].ChildNodes[1].ChildNodes;
                    for (int i = 0; i < ImgObj.Count(); i++) ImgUrls.Add(ImgObj[i].GetAttribute("href"));
                    saticiAdi = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].InnerText;
                    var NumbersList = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes;
                    for (int i = 0; i < NumbersList.Count(); i++) Contacts.Add(NumbersList[i].ChildNodes[0].InnerText);
                }
                else
                {
                    Seher = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText;
                    Marka = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText;
                    model = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[2].ChildNodes[1].InnerText;
                    Il = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerText;
                    BanNovu = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[4].ChildNodes[1].InnerText;
                    Reng = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[5].ChildNodes[1].InnerText;
                    Muherrik = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[6].ChildNodes[1].InnerText;
                    MuherrikGucu = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[7].ChildNodes[1].InnerText;
                    Yanacaq = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[8].ChildNodes[1].InnerText;
                    Yurus = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[9].ChildNodes[1].InnerText;
                    Karopka = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[10].ChildNodes[1].InnerText;
                    Oturucu = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[11].ChildNodes[1].InnerText;
                    Teze = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[12].ChildNodes[1].InnerText;
                    Qiymet = response.Css(".page-content > div")[0].ChildNodes[number].ChildNodes[num2].ChildNodes[1].ChildNodes[13].ChildNodes[1].InnerText;
                    var ImgObj = response.Css(".page-content > div")[0].ChildNodes[0].ChildNodes[1].ChildNodes;
                    for (int i = 0; i < ImgObj.Count(); i++) ImgUrls.Add(ImgObj[i].GetAttribute("href"));
                    saticiAdi = "dqwe";
                    saticiAdi = response.Css("div.seller-name")[0].InnerText;
                    Contacts.Add(response.Css("div.seller-phone")[0].InnerText);
                }
                var temps12 = response.Css("div.product-extras")[0].ChildNodes;
                for (int i = 0; i < temps12.Count(); i++) Extras.Add(temps12[i].InnerText);
                description = response.Css("h2.product-text")[0].InnerText;

                Yurus = Yurus.Remove(Yurus.Length - 2);


                Yurus = String.Concat(Yurus.Where(c => !Char.IsWhiteSpace(c)));

                string qiymetTipi;
                var temp = Qiymet.Split(' ');

                Qiymet = temp[0] + temp[1];

                qiymetTipi = temp[temp.Length - 1];

                qiymetTipi = qiymetTipi.Trim();

                Qiymet = Qiymet.Trim();

                Muherrik = Muherrik.Replace('.', ',');

                MuherrikGucu = MuherrikGucu.Substring(0, MuherrikGucu.Length - 4);

                string server = "localhost";
                string dbname = "cardealer";
                string user = "root";
                string pass = "";


                string conn = $"SERVER={server};DATABASE={dbname};UID={user};PWD={pass}";
                int test;
                if (Teze == "Xeyr")
                    test = 0;
                else
                    test = 1;
                using (var baglan = new MySqlConnection(conn))
                {
                    baglan.Open();

                    for (int i = 0; i < ImgUrls.Count; i++)
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            byte[] dataArr = webClient.DownloadData(ImgUrls[i]);
                            File.WriteAllBytes($"{IdTittle}-{i}.jpg", dataArr);
                        }
                    }
                    long lastCarid;

                    int cityId = 0;
                    //select query to my sql
                    string query = $"SELECT * FROM city WHERE CityName = '{Seher}'";
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, baglan);
                    //Execute command
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                        cityId = dataReader.GetInt32("id");
                    dataReader.Close();

                    int ColorId = 0;
                    //select query to my sql
                    string query1 = $"SELECT * FROM color WHERE ColorName = '{Reng}'";
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd1 = new MySqlCommand(query1, baglan);
                    dataReader = cmd1.ExecuteReader();

                    if (dataReader.Read())
                        ColorId = dataReader.GetInt32("id");

                    //close datareader
                    dataReader.Close();

                    int FuelId = 0;
                    //select query to my sql
                    string query2 = $"SELECT * FROM fuel WHERE FuelName = '{Yanacaq}'";
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd23 = new MySqlCommand(query2, baglan);
                    dataReader = cmd23.ExecuteReader();
                    if (dataReader.Read())
                        FuelId = dataReader.GetInt32("id");
                    dataReader.Close();

                    int GearId = 0;
                    //select query to my sql
                    string query3 = $"SELECT * FROM gearbox WHERE GearboxName = '{Karopka}'";

                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd3 = new MySqlCommand(query3, baglan);
                    dataReader = cmd3.ExecuteReader();
                    if (dataReader.Read())
                        GearId = dataReader.GetInt32("id");
                    dataReader.Close();
                    int CarTypeId = 0;
                    //select query to my sql
                    string query4 = $"SELECT * FROM cartype WHERE CarTypeName = '{BanNovu}'";
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd4 = new MySqlCommand(query4, baglan);
                    dataReader = cmd4.ExecuteReader();
                    if (dataReader.Read())
                        CarTypeId = dataReader.GetInt32("id");
                    dataReader.Close();


                    //SELECT id FROM employee_designation ORDER BY id DESC LIMIT 1;
                    query = $@"INSERT INTO `cars`(`ColorId`, `Fuelid`, `GearBoxId`, `CarTypeId`, `Make`, `Model`, `Year`, `Engine`, `EnginePower`, `MillAge`, `Price`, `PriceType`, `IsSalon`, `Description`)
                    VALUES ({ColorId},{FuelId},{GearId},{CarTypeId},'{Marka}','{model}','{Il}','{Muherrik}','{MuherrikGucu}','{Yurus}','{Qiymet}','{qiymetTipi}',{test},'{description}')";
                    using (cmd = new MySqlCommand(query, baglan))
                    {
                        cmd.ExecuteNonQuery();
                        lastCarid = cmd.LastInsertedId;
                    }
                    for (int i = 0; i < ImgUrls.Count; i++)
                    {
                        string newquery = $@"INSERT INTO `images`(`Value`, `CarId`) VALUES ('{IdTittle}-{i}.jpg',{lastCarid})";
                        using (var cmd2 = new MySqlCommand(newquery, baglan))
                        {
                            cmd2.ExecuteNonQuery();
                        }
                    }
                    for (int i = 0; i < Extras.Count; i++)
                    {
                        string newquery = $@"INSERT INTO `extras`(`Value`, `CarId`) VALUES ('{Extras[i]}',{lastCarid})";
                        using (var cmd2 = new MySqlCommand(newquery, baglan))
                        {
                            cmd2.ExecuteNonQuery();
                        }
                    }
                    for (int i = 0; i < Contacts.Count; i++)
                    {
                        string newquery = $@"INSERT INTO `contacts`(`PhoneNumber`, `CarId`) VALUES ('{Contacts[i]}',{lastCarid})";
                        using (var cmd2 = new MySqlCommand(newquery, baglan))
                        {
                            cmd2.ExecuteNonQuery();
                        }
                    }
                    Contacts.Clear();
                    Extras.Clear();
                    ImgUrls.Clear();


                    int userId = 1;
                    query = $@"INSERT INTO `elan`(`UserId`, `SellerName`, `CityId`, `CarId`, `Status`) VALUES ({userId},'{saticiAdi}',{cityId},{lastCarid},1)";
                    using (cmd = new MySqlCommand(query, baglan))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    
                }






                Console.WriteLine(saticiAdi);
                Console.WriteLine(Seher);
                Console.WriteLine(Marka);
                Console.WriteLine(model);
                Console.WriteLine(Il);
                Console.WriteLine(BanNovu);
                Console.WriteLine(Muherrik);
                Console.WriteLine(MuherrikGucu);
                Console.WriteLine(Yanacaq);
                Console.WriteLine(Yurus);
                Console.WriteLine(Karopka);
                Console.WriteLine(Oturucu);
                Console.WriteLine(Reng);
                Console.WriteLine(test);
                Console.WriteLine(Qiymet);
                Console.WriteLine(qiymetTipi);
                //using (SqlConnection sql = new SqlConnection(conn))
                //{
                //    sql.Open();


                //    string InsertString = @"INSERT Cars(Seher,Model,Il,BanNovu,Reng,Muherrik,Yanacaq,Yurus,Karopka,Oturucu,Teze,Qiymet,Marka,MuherrikGucu,QiymetTipi,ImageLink)
                //Values (@Seher,@Model,@Il,@Ban,@Reng,@Muh,@Yan,@Yurus,@Karop,@Otur,@Teze,@Qiymet,@Marka,@MuhGucu,@QiymetTipi,@Link)";



                //    var cmd = new SqlCommand(InsertString, sql);
                //    cmd.Parameters.Add("@Seher", SqlDbType.NVarChar).Value = Seher;
                //    cmd.Parameters.Add("@Model", SqlDbType.NVarChar).Value = model;
                //    cmd.Parameters.Add("@Il", SqlDbType.Int).Value = Il;
                //    cmd.Parameters.Add("@Ban", SqlDbType.NVarChar).Value = BanNovu;
                //    cmd.Parameters.Add("@Reng", SqlDbType.NVarChar).Value = Reng;
                //    cmd.Parameters.Add("@Muh", SqlDbType.Float).Value = float.Parse(Muherrik.Split(' ')[0]);
                //    cmd.Parameters.Add("@Yan", SqlDbType.NVarChar).Value = Yanacaq;
                //    cmd.Parameters.Add("@Yurus", SqlDbType.Float).Value = float.Parse(Yurus);
                //    cmd.Parameters.Add("@Karop", SqlDbType.NVarChar).Value = Karopka;
                //    cmd.Parameters.Add("@Otur", SqlDbType.NVarChar).Value = Oturucu;
                //    cmd.Parameters.Add("@Teze", SqlDbType.NVarChar).Value = Teze;
                //    cmd.Parameters.Add("@Qiymet", SqlDbType.Float).Value = float.Parse(Qiymet);
                //    cmd.Parameters.Add("@Marka", SqlDbType.NVarChar).Value = Marka;
                //    cmd.Parameters.Add("@MuhGucu", SqlDbType.Float).Value = float.Parse(MuherrikGucu.Split(' ')[0]);
                //    cmd.Parameters.Add("@QiymetTipi", SqlDbType.NVarChar).Value = qiymetTipi;
                //    cmd.Parameters.Add("@Link", SqlDbType.NVarChar).Value = ImageLink;

                //    cmd.ExecuteNonQuery();


                //}


            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    class Car
    {
        public string CityName { get; set; }

        public string Marka { get; set; }
        public string model { get; set; }
        public int Year { get; set; }
        public string BanNovu { get; set; }
        public string color { get; set; }
        public string engine { get; set; }
        public string EnginePower { get; set; }
        public string Yanacaq { get; set; }
        public string Yurus { get; set; }
        public string Transmision { get; set; }
        public string Oturucu { get; set; }
        public string New { get; set; }
        public string Price { get; set; }

    }

    class SiteScraper : WebScraper
    {

        public string URL { get; set; }
        public override void Init()
        {
            this.LoggingLevel = LogLevel.All;

            foreach (var ua in CommonUserAgents.All)
            {
                Identities.Add(new HttpIdentity
                {
                    UserAgent = ua,
                    UseCookies = true
                });
            }
            this.Request(URL, Parse);
        }
        public void JsonlToJson()
        {
            // var TextJson = JsonSerializer.Serialize(MyBooks, new JsonSerializerOptions() { WriteIndented = true });
            // File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Books.json", TextJson);

        }
        public List<string> Links { get; set; } = new List<string>();
        public override void Parse(Response response)
        {
            var temp = response.Css(".page-content > div")[0].ChildNodes[4].ChildNodes[1].ChildNodes;

            foreach (var item in temp)
            {
                var link = item.ChildNodes[0].GetAttribute("href");
                bool IsHave = false;
                for (int i = 0; i < Links.Count; i++)
                {
                    if (Links[i] == link)
                    {
                        IsHave = true;
                        break;
                    }
                }
                if (!IsHave) Links.Add(link);
            }

        }
        // Description
        //var tmp = response.Css(@".svelte-1a4s2ly")[0].ChildNodes[1].ChildNodes[2].ChildNodes[2];
        //LENGHT
        //var tmp = response.Css(@".svelte-1a4s2ly")[0].ChildNodes[1].ChildNodes[4].ChildNodes[0].ChildNodes[1].ChildNodes[0].CSS(".list-item value svelte-1at48u8").ChildNodes[2];
        //Console.WriteLine(tmp.InnerTextClean);
    }
}
