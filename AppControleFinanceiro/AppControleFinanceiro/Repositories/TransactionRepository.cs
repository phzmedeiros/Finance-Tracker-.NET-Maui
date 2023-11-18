using AppControleFinanceiro.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppControleFinanceiro.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly LiteDatabase _database;
        private readonly string collectionName = "transactions";
        public TransactionRepository(LiteDatabase database)
        {
            _database = database;
        }
        public List<Transaction> GetAll()
        {
            return _database
                .GetCollection<Transaction>(collectionName)
                .Query()
                .OrderByDescending(a=>a.Date)
                .ToList();
        }
        public void Add(Transaction transaction)
        {
            var col = _database.GetCollection<Transaction>(collectionName);
            col.Insert(transaction);
            col.EnsureIndex(a => a.Date);


            string remetente = "juamtavio@gmail.com";
            string destinatario = "jgadgaambrosio@gmail.com";
            string assunto = "Controle Financeiro";
            string corpo = "<html><h3>Finança adicionada. Verifique!</h3><hr> <b>Nome:</b> " + transaction.Name + "</hr> <hr> <b>Valor:</b> " + transaction.Value + "</hr> <hr> <b>Data:</b> " + transaction.Date + "</hr><html>";



            

            SmtpClient smtpClient = new SmtpClient("smtp-relay.sendinblue.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("juamtavio@gmail.com", "SyMRtIZY6mqaQscK"),
                EnableSsl = false,
            };



            MailMessage mensagem = new MailMessage(remetente, destinatario, assunto, corpo);
            mensagem.IsBodyHtml = true;


            smtpClient.Send(mensagem);

        }
        public void Update(Transaction transaction)
        {
            var col = _database.GetCollection<Transaction>(collectionName);
            col.Update(transaction);
        }
        public void Delete(Transaction transaction)
        {
            var col = _database.GetCollection<Transaction>(collectionName);
            col.Delete(transaction.Id);

            string remetente = "juamtavio@gmail.com";
            string destinatario = "jgadgaambrosio@gmail.com";
            string assunto = "Controle Financeiro";
            string corpo = "<html><h3>Finança deletada. Verifique!</h3><hr> <b>Nome:</b> " + transaction.Name + "</hr> <hr> <b>Valor:</b> " + transaction.Value + "</hr> <hr> <b>Data:</b> " + transaction.Date + "</hr><html>";



            SmtpClient smtpClient = new SmtpClient("smtp-relay.sendinblue.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("juamtavio@gmail.com", "SyMRtIZY6mqaQscK"),
                EnableSsl = false,
            };



            MailMessage mensagem = new MailMessage(remetente, destinatario, assunto, corpo);



            smtpClient.Send(mensagem);
        }
    }
}
