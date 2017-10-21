using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.MailToOmni
{
    public class Mail
    {
        /// <summary>
        /// 邮件接收人
        /// </summary>
        public string MailRecipient;

        /// <summary>
        /// 邮件主题
        /// </summary>
        public string MailSubject;

        /// <summary>
        /// 邮件主体
        /// </summary>
        public string MailBody;

        /// <summary>
        /// 邮件发件人邮箱
        /// </summary>
        public string MailFrom;

        /// <summary>
        /// 邮件发件人姓名
        /// </summary>
        public string MailFromName;

        public bool Send(MailToOmni.Mail email)
        {
            try
            {
                //邮件服务设置
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;


                Configuration config = Utils.GetConfig();
                AppSettingsSection appSection = (AppSettingsSection) config.GetSection("appSettings");
                smtpClient.Host = appSection.Settings["MailHost"].Value; //指定SMTP服务器
                string mailUser = appSection.Settings["MailUser"].Value;
                string mailPassword = appSection.Settings["MailPassword"].Value;
                smtpClient.Credentials = new System.Net.NetworkCredential(mailUser, mailPassword); //用户名和密码

                //发送邮件设置
                string mailRecipient = appSection.Settings["OmniMailDrop"].Value;
                MailMessage mailMessage = new MailMessage($"WoxMailToOmni<{mailUser}>", $"OmniMailDrop<{mailRecipient}>"); // 发送人和收件人
                //mailMessage.Bcc.Add("liq@gasgoo.com");
                mailMessage.Subject = email.MailSubject; //主题
                mailMessage.Body = email.MailBody; //内容
                mailMessage.BodyEncoding = Encoding.UTF8; //正文编码
                mailMessage.IsBodyHtml = true; //设置为HTML格式
                mailMessage.Priority = MailPriority.Normal; //优先级


                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
                //return false;
            }
        }
    }
}