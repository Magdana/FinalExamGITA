using GameGuessTheWord.Entities;
using System.Xml;

namespace GameGuessTheWord.Repositories;

internal class UserRepository
{
    private List<User> _users = new();
    private readonly XmlDocument _xmlDocument = new();
    public UserRepository(List<User> users)
    {
        _users = users;
        _xmlDocument = CreateXMLDoc(users);
    }

    private XmlDocument CreateXMLDoc(List<User> users)
    {
        var doc = new XmlDocument();
        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0",
            "UTF-8", string.Empty);
        doc.AppendChild(xmlDeclaration);
        XmlElement root = doc.CreateElement("Users");
        doc.AppendChild(root);
        foreach (var user in users)
        {
            XmlElement userEl = doc.CreateElement("user");
            root.AppendChild(userEl);

            XmlAttribute e = doc.CreateAttribute("name");
            e.Value = user.Name;
            userEl.Attributes.Append(e);

            XmlElement e2 = doc.CreateElement("userName");
            e2.InnerText = user.UserName;
            userEl.AppendChild(e2);

            XmlElement e3 = doc.CreateElement("password");
            e3.InnerText = user.Password;
            userEl.AppendChild(e3);

            XmlElement e4 = doc.CreateElement("higestScore");
            e3.InnerText = user.HigestScore.ToString();
            userEl.AppendChild(e3);
        }
        return doc;
    }

    public User? GetUserByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
    }
    public List<User> GetTopTenUsers()
    {
        return _users.FindAll(u => u.HigestScore.HasValue).OrderByDescending(u => u.HigestScore).Take(10).ToList();
    }
    public User AddUser(User user)
    {
        if (GetUserByUsername(user.UserName) != null)
        {
            throw new Exception("User with the same username already exists.");
        }
        if (string.IsNullOrWhiteSpace(user.Name) ||
            string.IsNullOrWhiteSpace(user.UserName) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            throw new Exception("User properties cannot be null or empty.");
        }
        _users.Add(user);
        return user;
    }
    public void UpdateUserScore(User user, int newScore) 
    {
        _users = _users.Select(u =>
        {
            if (GetUserByUsername(user.UserName) != null)
            {
                if (!u.HigestScore.HasValue || newScore < u.HigestScore.Value)
                {
                    u.HigestScore = newScore;
                }
            }
            return u;
        }).ToList();
    }

}
