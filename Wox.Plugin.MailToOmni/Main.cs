using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wox.Plugin.MailToOmni
{
    public class Main : IPlugin
    {
        private PluginInitContext _context;
        private string _separator;

        

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            const string ico = @"Images\\icon.png";
           

            if (query.Search.Length == 0)
            {
                results.Add(new Result
                {
                    Title = "请输入任务",
                    SubTitle = $"用“{_separator}”分隔标题和注释",
                    IcoPath = ico
                });
                return results;
            }
            else
            {
                var q = query.Search;
                results.Add(new Result
                {
                    Title = "回车发送任务",
                    SubTitle = $"任务：{q}",
                    IcoPath = ico,
                    Action = this.SendToOmniFunc(q)
                });
                return results;
            }
        }

        public void Init(PluginInitContext context)
        {
            _context = context;
            Configuration config = Utils.GetConfig();
            AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");
            _separator = appSection.Settings["Separator"].Value;
        }

        private System.Func<ActionContext, bool> SendToOmniFunc(string text)
        {
            return c =>
            {
                _context.API.ShowMsg(this.SendToOmin(text) ? "发送成功" : "发送失败");
                return false;
            };
        }

        private bool SendToOmin(string text)
        {
            var q = text;
            var title="";
            var comment="";
            int pos = q.IndexOf(_separator, StringComparison.OrdinalIgnoreCase);
            if (pos > 0)
            {
                title = q.Substring(0, pos);
                comment = q.Substring(pos+1, q.Length - pos-1);
            }
            else
            {
                title = q;
            }

            var e = new Mail
            {
                MailSubject = title,
                MailBody = comment
            };
            var ret = e.Send(e);
            return ret;
        }
    }
}
