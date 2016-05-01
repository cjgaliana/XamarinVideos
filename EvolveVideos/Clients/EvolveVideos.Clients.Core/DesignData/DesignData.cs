using System.Collections.Generic;
using EvolveVideos.Data.Models;

namespace EvolveVideos.Clients.UWP.DesignData
{
    public static class DesignData
    {
        public static EvolveSession GetSession()
        {
            return new EvolveSession()
            {
                Title = "A Behind the Scenes Insights into Building Technology for the Hearing Impaired",
                Track = "Mobile Ecosystem",
                Thumbnail = "http://img.youtube.com/vi/Xt5Is4y5iOU/hqdefault.jpg",
                Author = "Avichal Sharma, Serhat Aydin — Accenture",
                Description = "Accenture and Siemens are paving the way in medical mobile technology. The Siemens easyTek suite of apps enables the hearing impaired to connect to a variety of Bluetooth enabled devices. It also enables them to control the function and comfort of their in-ear hearing instrument. This real-world case study will show you how Accenture built the best mobile technology for the hearing impaired using Xamarin. They will cover the technical lessons learned from their experience, including connecting to multiple hearing device types, working with Android Bluetooth and MFI Accessory, and how to best incorporate continuous integration into the mobile development workflow.",
                YoutubeID = "Xt5Is4y5iOU"
            };
        }

        public static List<EvolveSession> GetSessions()
        {
            return new List<EvolveSession>
                {
                    new EvolveSession
                    {
                        Title = "A Behind the Scenes Insights into Building Technology for the Hearing Impaired",
                        Track = "Mobile Ecosystem",
                        Thumbnail = "http://img.youtube.com/vi/Xt5Is4y5iOU/hqdefault.jpg",
                        Author = "Avichal Sharma, Serhat Aydin — Accenture",
                        Description =
                            "Accenture and Siemens are paving the way in medical mobile technology. The Siemens easyTek suite of apps enables the hearing impaired to connect to a variety of Bluetooth enabled devices. It also enables them to control the function and comfort of their in-ear hearing instrument. This real-world case study will show you how Accenture built the best mobile technology for the hearing impaired using Xamarin. They will cover the technical lessons learned from their experience, including connecting to multiple hearing device types, working with Android Bluetooth and MFI Accessory, and how to best incorporate continuous integration into the mobile development workflow.",
                        YoutubeID = "Xt5Is4y5iOU"
                    },
                    new EvolveSession
                    {
                        Title = "A Canon in C#",
                        Track = "Platform",
                        Thumbnail = "http://img.youtube.com/vi/Nv-l4mArFT8/hqdefault.jpg",
                        Author = "Jon Skeet — Google",
                        Description =
                            "Jon Skeet is a software engineer at Google in London with a lingering passion for C# and its community. You'll often find Jon writing about C# in his blog, or his books, or even on Stack Overflow. He loves twisting the language into knots, often to the dismay of those he's presenting the code to!",
                        YoutubeID = "Nv-l4mArFT8"
                    },
                    new EvolveSession
                    {
                        Title = "Amazon Developer Ecosystem",
                        Track = "Emerging Devices",
                        Thumbnail = "http://img.youtube.com/vi/XJtmSvjx95c/hqdefault.jpg",
                        Author = "David Isbitski — Amazon",
                        Description =
                            "David Isbitski, Worldwide Developer Evangelist for the Amazon Appstore and Fire devices, wants to educate you on how to build native Amazon Fire phone, Fire TV, and Kindle Fire Apps with Xamarin. He'll give you a walk-through of how to quickly use Xamarin to run your apps and games on Amazon devices, as well as provide an overview of Amazon Appstore services that help developers get their app discovered and increase customer engagement and monetization.",
                        YoutubeID = "XJtmSvjx95c"
                    }
                };
        }
    }
}