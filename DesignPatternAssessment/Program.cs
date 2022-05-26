using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternAssessment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Ticket newTicket = new BugReport();
            newTicket.ErrorCode = "404";
            newTicket.ErrorLog = "404 encountered at unknown";

            newTicket = new HighPriority(newTicket);
            newTicket = new BugReportType(newTicket);

            newTicket = new WhiteGloveClient(newTicket);

            Console.WriteLine(newTicket.ResponseDeadlineHours());

            Console.ReadLine();
        }
    }

    public abstract class Ticket
    {
        public string ErrorCode { get; set; }
        public string ErrorLog { get; set; }
        public ServiceRequestEnumType RequestType { get; set; }

        public DateTime ResponseDeadline { get; set; }
        public DateTime BreachDeadline { get; set; }
        protected int _responseDeadlineHours { get; set; }
        protected int _breachDeadlineHours { get; set; }
        
        public virtual int ResponseDeadlineHours()
        {
            return _responseDeadlineHours;
        }

        public virtual int BreachDeadlineHours()
        {
            return _breachDeadlineHours;
        }
    }

    public abstract class Modifier : Ticket
    {
        public Ticket Ticket { get; set; }

        public abstract override int ResponseDeadlineHours();
        public abstract override int BreachDeadlineHours();
    }

    public class WhiteGloveClient : Modifier
    {
        public WhiteGloveClient(Ticket ticket)
        {
            Ticket = ticket;
        }

        public override int ResponseDeadlineHours()
        {
            return Ticket.ResponseDeadlineHours() * 3;
        }

        public override int BreachDeadlineHours()
        {
            return Ticket.BreachDeadlineHours() * 3;
        }
    }

    public class BacklogReissue : Modifier
    {
        public BacklogReissue(Ticket ticket)
        {
            Ticket = ticket;
        }

        public override int ResponseDeadlineHours()
        {
            return Ticket.ResponseDeadlineHours() + 100;
        }

        public override int BreachDeadlineHours()
        {
            return Ticket.BreachDeadlineHours() + 100;
        }
    }

    public abstract class PriorityCalculate : Ticket
    {
        public Ticket Ticket { get; set; }

        public abstract override int ResponseDeadlineHours();
        public abstract override int BreachDeadlineHours();
    }

    public abstract class TypeCalculate : Ticket
    {
        public Ticket Ticket { get; set; }

        public abstract override int ResponseDeadlineHours();
        public abstract override int BreachDeadlineHours();
    }

    public class HighPriority : PriorityCalculate
    {
        public HighPriority(Ticket ticket)
        {
            Ticket = ticket;
        }

        public override int ResponseDeadlineHours()
        {
            return Ticket.ResponseDeadlineHours() + 1;
        }

        public override int BreachDeadlineHours()
        {
            return Ticket.BreachDeadlineHours() + 1;
        }
    }

    public class BugReportType : TypeCalculate
    {
        public BugReportType(Ticket ticket)
        {
            Ticket = ticket;
        }

        public override int ResponseDeadlineHours()
        {
            return Ticket.ResponseDeadlineHours() * 2;
        }

        public override int BreachDeadlineHours()
        {
            return Ticket.BreachDeadlineHours() * 2;
        }
    }

    public class BugReport : Ticket
    {
        public BugReport()
        {
            _responseDeadlineHours = 1;
            _breachDeadlineHours = 1;
        }
    }

    public class ServiceRequest : Ticket
    {
        public ServiceRequest(ServiceRequestEnumType type)
        {
            RequestType = type;
        }
    }

    public enum ServiceRequestEnumType
    {
        GeneralQuestion,
        AccountIssue
    };

    public abstract class TicketListing
    {
        protected List<Ticket> _tickets = new List<Ticket>();
        public ServiceRequestEnumType type { get; set; }

        public TicketListing()
        {
            this.CreateTickets();
        }

        public virtual List<Ticket> Tickets()
        {
            return _tickets;
        }

        public abstract void CreateTickets();
    }

    public class BugReportTickets : TicketListing
    {
        public override void CreateTickets()
        {
            Tickets().Add(new BugReport());
        }
    }

    public class ServiceRequestTickets : TicketListing
    {
        public override void CreateTickets()
        {
            Tickets().Add(new ServiceRequest(type));
        }
    }
}
