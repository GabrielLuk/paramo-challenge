using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using Application.Errors;
using System.Net;
using Domain.Enums;
using System.Linq;
using System.Text;

namespace Application.User
{

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            List<Domain.User> _users = new List<Domain.User>();
            var newUser = new Domain.User
            {
                Name = request.User.Name,
                Email = request.User.Email,
                Address = request.User.Address,
                Phone = request.User.Phone,
                UserType = request.User.UserType,
                Money = request.User.Money
            };

            UserTypeEnum userTypeValue;
            if (!Enum.TryParse(newUser.UserType, out userTypeValue)) throw new RestException(HttpStatusCode.BadRequest, new { Message = "Invalid User Type." });
            
            if (userTypeValue == UserTypeEnum.Normal)
            {
                if (request.User.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    var gif = request.User.Money * percentage;
                    newUser.Money = newUser.Money + gif;
                }
                if (request.User.Money < 100)
                {
                    if (request.User.Money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = request.User.Money * percentage;
                        newUser.Money = newUser.Money + gif;
                    }
                }
            }
            if (userTypeValue == UserTypeEnum.SuperUser)
            {
                if (request.User.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    var gif = request.User.Money * percentage;
                    newUser.Money = newUser.Money + gif;
                }
            }
            if (userTypeValue == UserTypeEnum.Premium)
            {
                if (request.User.Money > 100)
                {
                    var gif = request.User.Money * 2;
                    newUser.Money = newUser.Money + gif;
                }
            }


            var reader = ReadUsersFromFile();
            //Normalize email
            var aux = newUser.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            newUser.Email = string.Join("@", new string[] { aux[0], aux[1] });

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = new Domain.User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                _users.Add(user);
            }
            reader.Close();

            var isDuplicated = _users.Count(x => x.Email == newUser.Email || x.Phone == newUser.Phone) > 0;
            if (isDuplicated) throw new RestException(HttpStatusCode.BadRequest, new { Message = "User is duplicated." });
            
            var userString = newUser.Name + ',' + newUser.Email + ',' + newUser.Phone + ',' + newUser.Address + ',' + newUser.UserType + ',' + newUser.Money;
            WriteUserFile(userString);
            
            return  true;

        }


        private StreamReader ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }

        private void WriteUserFile(string line)
        {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "/Files/Users.txt",true))
                {
                    StringBuilder sbFileContent = new StringBuilder();
                    sbFileContent.Append(line);
                    sbFileContent.Append("\n");
                    writer.Write(sbFileContent.ToString());
                }
        }
    }
}
