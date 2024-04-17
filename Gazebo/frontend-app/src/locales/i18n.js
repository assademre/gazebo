import i18n from 'i18next';
import Backend from 'i18next-http-backend'
import LanguageDetector from 'i18next-browser-languagedetector'
import { initReactI18next } from 'react-i18next';
import enTranslation from './en.json';
import deTranslation from './de.json';
import trTranslation from './tr.json';
import plTranslation from './pl.json';

i18n
  .use(Backend)
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    lng: 'en',
    detection: {
      order: ['queryString', 'cookie'],
      cache: ['cookie']
    },
    resources: {
      en: {
        translation: enTranslation,
      },
      de: {
        translation: deTranslation,
      },
      tr: {
        translation: trTranslation,
      },
      pl: {
        translation: plTranslation,
      }
    },
    keySeparator: false,
    debug: true,
    ns: ['translation'],
    fallbackLng: 'en',
    interpolation: {
      escapeValue: false,
    },
  });

export default i18n;
