using Bogus;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile($"{Environment.CurrentDirectory}\\appsettings.json")
                                       .Build();
int id = 1;
var faker = new Faker<People>()//.RuleFor(p => p.PersonId,
                               //         f => id++)
                               .RuleFor(p => p.GivenNames,
                                        Faker.NameFaker.FirstName)
                               .RuleFor(p => p.Surname,
                                        Faker.NameFaker.LastName)
                               .RuleFor(p => p.AddressLine1,
                                        String.Format("{0} {1}", Faker.LocationFaker.StreetNumber(), Faker.LocationFaker.StreetName()))
                               .RuleFor(p => p.AddressLine2,
                                        Faker.CompanyFaker.Name)
                               .RuleFor(p => p.City,
                                        Faker.LocationFaker.City())
                               .RuleFor(p => p.State,
                                        "Tasmania")
                               .RuleFor(p => p.PostCode,
                                        Faker.LocationFaker.ZipCode());
//List<People> people = faker.Generate(120000000);
using(DemoContext context = new DemoContext(config.GetConnectionString("Sql")))
{
    for (int i = 0; i < 300; i++)
    {
        context.AddRange(faker.Generate(40000));
        context.SaveChanges();
    }
}
// List<Model> models = new List<Model>();
// int id = 1;

// models.AddRange(faker.Generate(1000));

// string json = JsonSerializer.Serialize(models);
// using (FileStream fs = File.Create(config["OutputFile"]!))
// {
//     using (StreamWriter sw = new StreamWriter(fs))
//     {
//         sw.Write(json);
//     }
// }