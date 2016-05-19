using Newtonsoft.Json;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using EntityFramework.BulkInsert.Extensions;
using System.Transactions;
using PhoneBook.Services;
using PhoneBook.Repositories;

namespace PhoneBook.App_Start
{
    public static class LocationConfig
    {
        public static void ParseData()
        {
            List<Country> allCountries = new List<Country>();
            CountriesService countriesService = new CountriesService();
           
            using (WebClient webClient = new WebClient())
            {
                string jsonString = webClient.DownloadString("https://raw.githubusercontent.com/David-Haim/CountriesToCitiesJSON/master/countriesToCities.json");
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonString);

                Country country;
                foreach (var item in dictionary)
                {
                    country = new Country();
                    country.Name = item.Key;
                    allCountries.Add(country);
                }

                countriesService.InsertCollection(allCountries);

                List<Country> insertedCountries = countriesService.GetAll();
                List<City> cities = new List<City>();
                CitiesService citiesService = new CitiesService();
                foreach (var item in dictionary)
                {
                    foreach (Country c in insertedCountries)
                    {
                        if (c.Name == item.Key)
                        {
                            cities.AddRange(item.Value.Select(city => new City() { Name = city, CountryID = c.ID }));
                        }
                    }
                }

                citiesService.InsertCollection(cities);
            }
        }
    }
}