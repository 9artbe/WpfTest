using System;
using System.Linq;
using WpfTest.Models;

namespace WpfTest.Data
{
    public class DbInitializer
    {
        public static void Initialize(WorkDbContext context)
        {
            if(!context.Database.EnsureCreated())
                return;

            if (!context.Departments.Any())
            {
                var departments = new Department[]
                {
                    new Department() {Name = "Инженерно-технический отдел"},
                    new Department() {Name = "Отдел кадров"}
                };

                foreach (var department in departments)
                {
                    context.Departments.Add(department);
                }

                context.SaveChanges();
            }

            if (!context.Persons.Any())
            {
                var persons = new Person[]
                {
                    new Person()
                    {
                        LastName = "Иванов", FirstName = "Александр", SecondName = "Андреевич",
                        BirthDate = new DateTime(1954, 08, 30), Gender = GenderType.Male, DepartmentId = 1
                    },
                    new Person()
                    {
                        LastName = "Сидорова", FirstName = "Юлия", SecondName = "Алексеевна",
                        BirthDate = new DateTime(1980, 03, 20), Gender = GenderType.Female, DepartmentId = 2
                    },
                    new Person()
                    {
                        LastName = "Петров", FirstName = "Владимир", SecondName = "Сергеевич",
                        BirthDate = new DateTime(2015, 12, 10), Gender = GenderType.Male, DepartmentId = 1
                    }
                };


                foreach (var person in persons)
                {
                    context.Persons.Add(person);
                }

                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                var orders = new Order[]
                {
                    new Order() {OrderNumber = "4585", ContractorName = "ООО Искра", OrderDate = new DateTime(2020, 12, 05)},
                    new Order()
                    {
                        OrderNumber = "1257-A", ContractorName = "ИП Данилов", OrderDate = new DateTime(2021, 01, 17)
                    },
                    new Order() {OrderNumber = "3507", ContractorName = "ООО Искра", OrderDate = new DateTime(2021, 02, 05)},
                };

                foreach (var order in orders)
                {
                    context.Orders.Add(order);
                }

                context.SaveChanges();
            }
        }
    }
}