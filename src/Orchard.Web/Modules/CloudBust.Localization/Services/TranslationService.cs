using System.Collections.Generic;
using System.Linq;
using CloudBust.Localization.Models;
using Orchard.Data;

namespace CloudBust.Localization.Services {
    public class TranslationService : ITranslationService {
        private readonly IRepository<TranslationRecord> _translationsRepository;

        public TranslationService(
            IRepository<TranslationRecord> translationsRepository
        ) {
            _translationsRepository = translationsRepository;
        }

        public TranslationRecord CreateTranslation(string name, string twoDigitCode) {
            _translationsRepository.Create(new TranslationRecord {
                Name = name,
                TwoDigitCode = twoDigitCode
            });

            // rare object, return immediately to commit changes
            var translation = GetTranslation(name);

            return translation;
        }

        public bool DeleteTranslation(int id) {
            var translation = GetTranslation(id);
            if (translation == null) return false;
            _translationsRepository.Delete(translation);

            return true;
        }

        public TranslationRecord GetTranslation(int id) {
            try {
                var translation = _translationsRepository.Get(id);
                return translation;
            }
            catch {
                return null;
            }
        }

        public TranslationRecord GetTranslation(string name) {
            try {
                var translations = from translation in _translationsRepository.Table where translation.Name == name select translation;
                return translations.FirstOrDefault();
            }
            catch {
                return null;
            }
        }

        public TranslationRecord GetTranslationByCode(string twoDigitCode) {
            try {
                var translations = from translation in _translationsRepository.Table where translation.TwoDigitCode == twoDigitCode select translation;
                return translations.FirstOrDefault();
            }
            catch {
                return null;
            }
        }

        public string GetTranslationCode(int id) {
            var translation = GetTranslation(id);
            return translation?.TwoDigitCode;
        }

        public string GetTranslationCode(string name) {
            var translation = GetTranslation(name);
            return translation?.TwoDigitCode;
        }

        public string GetTranslationName(string twoDigitCode) {
            var translation = GetTranslationByCode(twoDigitCode);
            return translation?.Name;
        }

        public string GetTranslationName(int id) {
            var translation = GetTranslation(id);
            return translation?.Name;
        }

        public IEnumerable<TranslationRecord> GetTranslations() {
            try {
                var translations = from translation in _translationsRepository.Table select translation;
                return translations.ToList();
            }
            catch {
                return new List<TranslationRecord>();
            }
        }

        public bool UpdateTranslation(int id, string name, string twoDigitCode) {
            var translation = GetTranslation(id);
            if (translation == null) return false;

            translation.Name = name;
            translation.TwoDigitCode = twoDigitCode;
            return true;
        }

        public bool UpdateTranslationCode(int id, string twoDigitCode) {
            var translation = GetTranslation(id);
            if (translation == null) return false;

            translation.TwoDigitCode = twoDigitCode;
            return true;
        }

        public bool UpdateTranslationName(int id, string name) {
            var translation = GetTranslation(id);
            if (translation == null) return false;

            translation.Name = name;
            return true;
        }

        public bool UpdateTranslationCode(string name, string twoDigitCode) {
            var translation = GetTranslation(name);
            if (translation == null) return false;

            translation.TwoDigitCode = twoDigitCode;
            return true;
        }
    }
}