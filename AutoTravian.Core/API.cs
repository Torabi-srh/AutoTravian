using AutoTravian.Core.Data;
using AutoTravian.Core.Enums;
using AutoTravian.Core.Util;
using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;

namespace AutoTravian.Core
{
    public class API
    {
        private readonly SocketsHttpHandler webHandler;
        private readonly HttpClient webWorker;
        public delegate void ResourcesEventHandler(Resources sender, EventArgs e);
        public event ResourcesEventHandler OnResourcesUpdate;

        public delegate void ResourceFieldsEventHandler(Dictionary<int, Building> sender, EventArgs e);
        public event ResourceFieldsEventHandler OnResourceFieldsUpdate;

        public delegate void BuildingsEventHandler(Dictionary<int, Building> sender, EventArgs e);
        public event BuildingsEventHandler OnBuildingsUpdate;

        public delegate void VilageInfoEventHandler(List<Vilage> sender, EventArgs e);
        public event VilageInfoEventHandler OnVilageInfoUpdate;
        private string baseUrl = "ttwars.com";
        private bool loggedIn;
        public bool Active = false;
        public API(string url)
        {
            webHandler = new();
            webWorker = new(webHandler);
            HelpTools.CoInternetSetFeatureEnabled();
            HelpTools.UrlMkSetSessionOption("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            //WinInetInterop.SetConnectionProxy("127.0.0.1:7777");
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.ServerCertificateValidationCallback += (senderj, cert, chain, sslPolicyErrors) => true;
            loggedIn = false;
            webHandler.AllowAutoRedirect = true;
            webHandler.ConnectTimeout = TimeSpan.FromMilliseconds(15 * 1000);
            webHandler.KeepAlivePingPolicy = HttpKeepAlivePingPolicy.Always;
            webHandler.UseCookies = true;
            webHandler.SslOptions.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls;
            webWorker.Timeout = TimeSpan.FromMilliseconds(15 * 1000);
            webWorker.DefaultRequestHeaders.Accept.Clear();
            webWorker.DefaultRequestHeaders.UserAgent.Clear();
            if (!string.IsNullOrEmpty(url)) Init(url);
        }
        public void Init(string url)
        {
            baseUrl = url.Replace("https", string.Empty).Replace("http", string.Empty).Replace("://", string.Empty).Split("/")[0];
            webWorker.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0");
            webWorker.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            webWorker.DefaultRequestHeaders.Add("Connection", "keep-alive");
            webWorker.DefaultRequestHeaders.Add("Referer", $"https://{baseUrl}/");
            webWorker.DefaultRequestHeaders.Add("Origin", $"https://{baseUrl}/");
            webWorker.DefaultRequestHeaders.Add("TE", "trailers");
            webWorker.DefaultRequestHeaders.Add("Host", baseUrl);
        }
        private async Task Delay()
        {
            Random rand = new Random();
            int delay = rand.Next(2000, 5000);
            await Task.Delay(delay);
        }
        private async Task<string> getToken()
        {
            await Delay();
            string url = $"https://{baseUrl}/index.php";
            var response = await webWorker.GetAsync(url);
            Stream stream = await response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
            var nodes = doc.DocumentNode.SelectSingleNode("//input[@name='login']");
            return nodes?.GetAttributeValue("value", "") ?? "";
        }
        public async Task<bool> Login(string user, string passwd)
        {
            await Delay();
            if (loggedIn)
            {
                string? time = await ServerTime(null);
                if (string.IsNullOrEmpty(time))
                {
                    loggedIn = false;
                }
                else
                {
                    loggedIn = true;
                }
                return false;
            }
            string url = $"https://{baseUrl}/index.php";
            string token = await getToken();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("name", user),
                new KeyValuePair<string, string>("password", passwd),
                new KeyValuePair<string, string>("s1", "Login"),
                new KeyValuePair<string, string>("w", "1366:768"),
                new KeyValuePair<string, string>("login", token),
            });
            var response = await webWorker.PostAsync(url, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStreamAsync();
                string? time = await ServerTime(responseContent);
                if (string.IsNullOrEmpty(time))
                {
                    loggedIn = false;
                }
                else
                {
                    loggedIn = true;
                }
            }
            return loggedIn;
        }
        public async Task<string?> ServerTime(Stream? stream)
        {
            await Delay();
            if (stream is null)
            {
                string url = $"https://{baseUrl}/dorf1.php";
                var response = await webWorker.GetAsync(url);
                stream = await response.Content.ReadAsStreamAsync();
            }
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
            var nodes = doc.DocumentNode.SelectSingleNode("//div[@id='servertime']/span");
            return nodes?.GetAttributeValue("value", null);
        }
        public async Task<bool> Resources()
        {
            await Delay();
            string url = $"https://{baseUrl}/dorf1.php";
            var response = await webWorker.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);

            Resources r = new();
            r.Warehouse = doc.DocumentNode.SelectSingleNode("(//div[@class='value'])[1]")?.InnerText;
            r.Wood = doc.DocumentNode.SelectSingleNode("(//div[@class='value'])[2]")?.InnerText;
            r.Clay = doc.DocumentNode.SelectSingleNode("(//div[@class='value'])[3]")?.InnerText;
            r.Iron = doc.DocumentNode.SelectSingleNode("(//div[@class='value'])[4]")?.InnerText;
            r.Granary = doc.DocumentNode.SelectSingleNode("(//div[@class='value'])[5]")?.InnerText;
            r.Crop = doc.DocumentNode.SelectSingleNode("(//div[@class='value '])[1]")?.InnerText;
            r.Gold = doc.DocumentNode.SelectSingleNode("//div[@class='value ajaxReplaceableGoldAmount']")?.InnerText;
            r.Crop = doc.DocumentNode.SelectSingleNode("//div[@class='value ajaxReplaceableSilverAmount']")?.InnerText;
            OnResourcesUpdate(r, EventArgs.Empty);
            return true;
        }
        public async Task<bool> ResourceFields()
        {
            Dictionary<int, Building> r = new();
            int max = 18;
            int ci = 1;
            while (ci <= max)
            {
                await Delay();
                if (ci > 40) break;
                string url = $"https://{baseUrl}/build.php?id=1";
                var response = await webWorker.GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                HtmlDocument doc = new HtmlDocument();
                doc.Load(stream);
                string? resource = doc.DocumentNode.SelectSingleNode("//h1[@class='titleInHeader']")?.InnerText;
                string? level = doc.DocumentNode.SelectSingleNode("//span[@class='level']")?.InnerText;
                if (string.IsNullOrEmpty(resource)) continue;
                if (string.IsNullOrEmpty(level)) continue;
                float product = float.TryParse(doc.DocumentNode.SelectSingleNode("(//span[@class='number'])[1]")?.InnerText, out product) ? product : 0;
                float nextProduct = float.TryParse(doc.DocumentNode.SelectSingleNode("(//span[@class='number'])[2]")?.InnerText, out nextProduct) ? nextProduct : 0;
                string pattern = @"[^\d]";
                int ilevel = int.TryParse(Regex.Replace(level, pattern, ""), out ilevel) ? ilevel : 0;
                switch (resource.ToLower())
                {
                    case var res when res.StartsWith("wood"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Wood,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.StartsWith("crop"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Crop,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.StartsWith("iron"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Iron,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.StartsWith("clay"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Clay,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    default:
                        continue;
                }
                ci++;
            }
            OnResourceFieldsUpdate(r, EventArgs.Empty);
            return true;
        }
        public async Task<bool> Buildings()
        {
            Dictionary<int, Building> r = new();
            int max = 40;
            int ci = 19;
            while (ci <= max)
            {
                await Delay();
                if (ci > 40) break;
                string url = $"https://{baseUrl}/build.php?id=1";
                var response = await webWorker.GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                HtmlDocument doc = new HtmlDocument();
                doc.Load(stream);
                string? resource = doc.DocumentNode.SelectSingleNode("//h1[@class='titleInHeader']")?.InnerText;
                string? level = doc.DocumentNode.SelectSingleNode("//span[@class='level']")?.InnerText;
                if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(level))
                {
                    r.Add(ci, new Building()
                    {
                        id = ci,
                        Field = BuildingTypes.Empty,
                        Level = 0,
                        Name = "Empty",
                        Production = 0,
                        ProductionNextLevel = 0
                    });
                    ci++;
                    continue;
                }
                float product = float.TryParse(doc.DocumentNode.SelectSingleNode("(//span[@class='number'])[1]")?.InnerText, out product) ? product : 0;
                float nextProduct = float.TryParse(doc.DocumentNode.SelectSingleNode("(//span[@class='number'])[2]")?.InnerText, out nextProduct) ? nextProduct : 0;
                string pattern = @"[^\d]";
                int ilevel = int.TryParse(Regex.Replace(level, pattern, ""), out ilevel) ? ilevel : 0;
                switch (resource.ToLower())
                {

                    case var res when res.Contains("main building "):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.MainBuilding,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("barracks"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Barracks,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("grain mill"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.GrainMill,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("foundry"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Foundry,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("trade office"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.TradeOffice,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("sawmill"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Sawmill,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("academy"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Academy,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("bakery"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Bakery,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("brewery"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Brewery,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("brickyard"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Brickyard,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("granary"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Granary,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("wall"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Wall,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("warehouse"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Warehouse,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("treasury"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Treasury,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("tournament"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Tournament,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("stonemason"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Stonemason,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("town hall"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.TownHall,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("palace"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Palace,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("residence"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Residence,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("marketplace"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Marketplace,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("cranny"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Cranny,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("mansion"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Mansion,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("smithy"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Smithy,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("stable"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Stable,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    case var res when res.Contains("workshop"):
                        r.Add(ci, new Building()
                        {
                            id = ci,
                            Field = BuildingTypes.Workshop,
                            Level = ilevel,
                            Name = resource,
                            Production = product,
                            ProductionNextLevel = nextProduct
                        });
                        break;
                    default:
                        continue;
                }
                ci++;
            }
            OnBuildingsUpdate(r, EventArgs.Empty);
            return true;
        }
        public async Task<bool> VilageInfo()
        {
            await Delay();
            List<Vilage> vliages = new List<Vilage>();
            string url = $"https://{baseUrl}/village/statistics";
            var response = await webWorker.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
            var nodes = doc.DocumentNode.SelectNodes("//table[@id='overview']//tbody//tr");
            foreach (var node in nodes)
            {
                vliages.Add(new()
                {
                    id = node.SelectSingleNode("//td[1]").FirstChild.GetAttributeValue("href", "0=0").Split("=")[1],
                    Name = node.SelectSingleNode("//td[1]").InnerText,
                    Attacks = node.SelectSingleNode("//td[2]").InnerText,
                    Building = node.SelectSingleNode("//td[3]").InnerText,
                    Troops = node.SelectSingleNode("//td[4]").InnerText,
                    Merchants = node.SelectSingleNode("//td[5]").InnerText,
                });
            }
            OnVilageInfoUpdate(vliages, EventArgs.Empty);
            return true;
        }
        public async Task<bool> BuildingUpgrade(string id)
        {
            await Delay();
            string url = $"https://{baseUrl}/build.php?id={id}";
            var response = await webWorker.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
            string node = doc.DocumentNode.SelectSingleNode("//button[contains(@class,'textButtonV1 green')]").GetAttributeValue("onclick", "");
            Match match = Regex.Match(node, @"href\s*=\s*'(.*?)'");
            if (match.Success)
            {
                node = match.Groups[1].Value;
                await Delay();
                url = $"https://{baseUrl}/{node}";
                response = await webWorker.GetAsync(url);
                if (response.IsSuccessStatusCode) return false;
            }
            return false;
        }
        public async Task<bool> TrainUnits(string id, string quantity)
        {
            await Delay();
            string url = $"https://{baseUrl}/build.php?id={id}";
            var response = await webWorker.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
            var nodes = doc.DocumentNode.SelectNodes("//form[@method='post']//input");

            List<KeyValuePair<string, string>> units = new List<KeyValuePair<string, string>>();
            foreach (var node in nodes)
            {
                var name = node.GetAttributeValue("name", "");
                var val = node.GetAttributeValue("value", "");
                if (name == "t1") val = quantity.ToString();
                units.Add(new KeyValuePair<string, string>(name, val));
            }
            units.Add(new KeyValuePair<string, string>("s1", "Train"));
            await Delay();
            var content = new FormUrlEncodedContent(units);
            url = $"https://{baseUrl}/build.php?id={id}";
            response = await webWorker.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> BuildBuilding(string id, BuildingTypes bt)
        {
            string btype = "";
            switch (bt)
            {
                case BuildingTypes.Empty:
                    btype = "";
                    break;
                case BuildingTypes.Iron:
                    btype = "";
                    break;
                case BuildingTypes.Wood:
                    btype = "";
                    break;
                case BuildingTypes.Crop:
                    btype = "";
                    break;
                case BuildingTypes.Clay:
                    btype = "";
                    break;
                case BuildingTypes.Cranny:
                    btype = "23";
                    break;
                case BuildingTypes.Warehouse:
                    btype = "10";
                    break;
                case BuildingTypes.Granary:
                    btype = "11";
                    break;
                case BuildingTypes.Marketplace:
                    btype = "";
                    break;
                case BuildingTypes.Residence:
                    btype = "";
                    break;
                case BuildingTypes.Palace:
                    btype = "";
                    break;
                case BuildingTypes.Stonemason:
                    btype = "";
                    break;
                case BuildingTypes.Treasury:
                    btype = "";
                    break;
                case BuildingTypes.TownHall:
                    btype = "";
                    break;
                case BuildingTypes.MainBuilding:
                    btype = "";
                    break;
                case BuildingTypes.TradeOffice:
                    btype = "";
                    break;
                case BuildingTypes.Brewery:
                    btype = "";
                    break;
                case BuildingTypes.Wall:
                    btype = "";
                    break;
                case BuildingTypes.Mansion:
                    btype = "37";
                    break;
                case BuildingTypes.Barracks:
                    btype = "";
                    break;
                case BuildingTypes.Academy:
                    btype = "22";
                    break;
                case BuildingTypes.Smithy:
                    btype = "";
                    break;
                case BuildingTypes.Stable:
                    btype = "";
                    break;
                case BuildingTypes.Workshop:
                    btype = "";
                    break;
                case BuildingTypes.Tournament:
                    btype = "";
                    break;
                case BuildingTypes.GrainMill:
                    btype = "";
                    break;
                case BuildingTypes.Sawmill:
                    btype = "";
                    break;
                case BuildingTypes.Brickyard:
                    btype = "";
                    break;
                case BuildingTypes.Foundry:
                    btype = "";
                    break;
                case BuildingTypes.Bakery:
                    btype = "";
                    break;
                default:
                    btype = "";
                    break;
            }
            if (string.IsNullOrEmpty(btype)) return false;
            await Delay();
            string url = $"https://{baseUrl}/build.php?id={id}";
            var response = await webWorker.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
            string node = doc.DocumentNode.SelectSingleNode("(//button[contains(@class,'textButtonV1 green')])[1]").GetAttributeValue("onclick", "");
            Match match = Regex.Match(node, @"dorf2\.php\?(.*)");
            if (match.Success)
            {
                string[] parts = match.Groups[1].Value.Split('&');
                Dictionary<string, string> values = new Dictionary<string, string>();
                foreach (string part in parts)
                {
                    string[] keyValue = part.Split('=');
                    values.Add(keyValue[0], keyValue[1]);
                }
                await Delay();
                url = $"https://{baseUrl}/dorf2.php?a={btype}&id={id}&c={values["c"]}";
                response = await webWorker.GetAsync(url);
                if (response.IsSuccessStatusCode) return true;
            }
            return false;
        }
    }
}