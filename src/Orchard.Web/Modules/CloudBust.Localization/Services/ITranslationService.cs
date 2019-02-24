using Orchard;
using System.Collections.Generic;
using CloudBust.Localization.Models;

namespace CloudBust.Localization.Services
{
    public interface ITranslationService : IDependency
    {
        TranslationRecord GetTranslation(int id);
        TranslationRecord GetTranslation(string name);
        TranslationRecord GetTranslationByCode(string twoDigitCode);
        string GetTranslationCode(int id);
        string GetTranslationCode(string name);
        string GetTranslationName(string twoDigitCode);
        string GetTranslationName(int id);
        IEnumerable<TranslationRecord> GetTranslations();
        TranslationRecord CreateTranslation(string name, string twoDigitCode);
        bool UpdateTranslation(int id, string name, string twoDigitCode);
        bool UpdateTranslationName(int id, string name);
        bool UpdateTranslationCode(int id, string twoDigitCode);
        bool DeleteTranslation(int id);
    }
}