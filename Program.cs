using System;
using Models;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lab5_Tema
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        static void Main(string[] args)
        {
            Console.WriteLine("Helloow");
        }

        static async Task Initilize()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;"
            + "AccountName=<Account Name>"
            + ";AccountKey=<Account Key>"
            + ";EndpointsSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("Studenti: ");

            await studentsTable.CreateIfNoExistsAsync();
            await AddNewStudent();
            await EditStudent();
            await GetAllStudents();
        }

        private static async Task AddNewStudent()
        {
            var student = new StudentEntity("UPT", "1950721237791");
            student.FirstName = "Dumitru";
            student.LastName = "Iarba";
            student.Email = "iarbadumitru@gmail.com";
            student.Year = 4;
            student.PhoneNumber = "07332344361";
            student.Faculty = "AC";

            var insertOperation = TableOperation.Insert(student);

            await studentTable.ExecuteAsync(insertOperation);

        }

        private static async Task GetAllStudent()
        {
            Console.WriteLine("Universitate\tCNP\tNume\tEmail\tNumar telefon\tAn");
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.TableContinuationToken;

                foreach (StudentEntity entity in resultSegment.Results)
                {
                    Console.WriteLine("{0}\t{1}\t{2} {3}\t{4}\t{5}\t{6}", entity.PartitionKey, entityRowKey, entity.FirstName, entityLastName, entity.Email, entity.PhoneNumber, entity.Year);
                } while (token != null) ;
            }
        }
    }
}