using HtmlAgilityPack;
using PuppeteerSharp;
using System.Net;
using System.Media;
using NAudio.Wave;

void PlayAlert()
{
    // Chemin du fichier WAV
    string cheminFichierWav = Path.Combine(AppContext.BaseDirectory, "alert.mp3");

    // Utiliser NAudio pour jouer un fichier WAV
    using (var audioFile = new AudioFileReader(cheminFichierWav))
    using (var lecteur = new WaveOutEvent())
    {
        lecteur.Init(audioFile);
        lecteur.Play();

        Console.WriteLine("Lecture du fichier WAV...");

        // Attendre la fin de la lecture
        while (lecteur.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(100);
        }
    }
}

async Task Index()
{
    string fullUrl = "https://ca.movember.com/fr/mospace/14783494";

    var programmerLinks = new List<string>();

    var options = new LaunchOptions()
    {
        Headless = true,
        ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
    };

    var browser = await Puppeteer.LaunchAsync(options, null);
    var page = await browser.NewPageAsync();
    await page.GoToAsync(fullUrl);

    var totalDonationsElement = await page.QuerySelectorAsync(".mospace-heroarea--donations-target-amount-number");
    var totalDonations = (await totalDonationsElement.GetPropertyAsync("innerText")).RemoteObject.Value.ToString();
    File.WriteAllText(@"totalDonations.txt", "Will c'est le plus hot");

    var listDonnations = await page.QuerySelectorAllAsync("div[id^=\"post-wrapper-\"][id$=\"-all\"]");

    var listDonnationTexts = new List<string>();

    foreach(var donnation in listDonnations)
    {
        var text = (await donnation.GetPropertyAsync("innerText")).RemoteObject.Value.ToString() ?? "";
        text += "\n";
        listDonnationTexts.Add(text);
    }

    File.Delete(@"donnateurs.txt");
    File.AppendAllLines(@"donnateurs.txt", listDonnationTexts);

    await browser.CloseAsync();

}

await Index();