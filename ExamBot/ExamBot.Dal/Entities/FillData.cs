using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBot.Dal.Entities;

public class FillData
{
    public long FillDataId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    

    public long BotUserId { get; set; }
    public User BotUser { get; set; }
}
