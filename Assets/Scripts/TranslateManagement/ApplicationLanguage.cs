﻿namespace TranslateManagement
{
    public enum ApplicationLanguage
    {
        Unknown,
        Afrikaans,
        Akan,
        Albanian,
        Amharic,
        Arabic,
        Armenian,
        Azerbaijani,
        Basque,
        Belarusian,
        Bemba,
        Bengali,
        Bihari,
        Bork,
        Bosnian,
        Breton,
        Bulgarian,
        Cambodian,
        Catalan,
        Cherokee,
        Chichewa,
        Chinese_Simplified,
        Chinese_Traditional,
        Corsican,
        Croatian,
        Czech,
        Danish,
        Dutch,
        Elmer,
        English,
        Esperanto,
        Estonian,
        Ewe,
        Faroese,
        Filipino,
        Finnish,
        French,
        Frisian,
        Ga,
        Galician,
        Georgian,
        German,
        Greek,
        Guarani,
        Gujarati,
        Hacker,
        Haitian,
        Hausa,
        Hawaiian,
        Hebrew,
        Hindi,
        Hungarian,
        Icelandic,
        Igbo,
        Indonesian,
        Interlingua,
        Irish,
        Italian,
        Japanese,
        Javanese,
        Kannada,
        Kazakh,
        Kinyarwanda,
        Kirundi,
        Klingon,
        Kongo,
        Korean,
        Krio,
        Kurdish,
        Kurdish_Soranî,
        Kyrgyz,
        Laothian,
        Latin,
        Latvian,
        Lingala,
        Lithuanian,
        Lozi,
        Luganda,
        Luo,
        Macedonian,
        Malagasy,
        Malay,
        Malayalam,
        Maltese,
        Maori,
        Marathi,
        Mauritian_Creole,
        Moldavian,
        Mongolian,
        Montenegrin,
        Nepali,
        Nigerian_Pidgin,
        Northern_Sotho,
        Norwegian,
        Norwegian_Nynorsk,
        Occitan,
        Oriya,
        Oromo,
        Pashto,
        Persian,
        Pirate,
        Polish,
        Portuguese_Brazil,
        Portuguese_Portugal,
        Punjabi,
        Quechua,
        Romanian,
        Romansh,
        Runyakitara,
        Russian,
        Scots,
        Serbian,
        Serbo_Croatian,
        Sesotho,
        Setswana,
        Seychellois_Creole,
        Shona,
        Sindhi,
        Sinhalese,
        Slovak,
        Slovenian,
        Somali,
        Spanish,
        Spanish_LatinAmerican,
        Sundanese,
        Swahili,
        Swedish,
        Tajik,
        Tamil,
        Tatar,
        Telugu,
        Thai,
        Tigrinya,
        Tonga,
        Tshiluba,
        Tumbuka,
        Turkish,
        Turkmen,
        Twi,
        Uighur,
        Ukrainian,
        Urdu,
        Uzbek,
        Vietnamese,
        Welsh,
        Wolof,
        Xhosa,
        Yiddish,
        Yoruba,
        Zulu,
    }
    /*
    hl=af          Afrikaans
    hl=ak          Akan
    hl=sq          Albanian
    hl=am          Amharic
    hl=ar          Arabic
    hl=hy          Armenian
    hl=az          Azerbaijani
    hl=eu          Basque
    hl=be          Belarusian
    hl=bem         Bemba
    hl=bn          Bengali
    hl=bh          Bihari
    hl=xx-bork     Bork, bork, bork!
    hl=bs          Bosnian
    hl=br          Breton
    hl=bg          Bulgarian
    hl=km          Cambodian
    hl=ca          Catalan
    hl=chr         Cherokee
    hl=ny          Chichewa
    hl=zh-CN       Chinese (Simplified)
    hl=zh-TW       Chinese (Traditional)
    hl=co          Corsican
    hl=hr          Croatian
    hl=cs          Czech
    hl=da          Danish
    hl=nl          Dutch
    hl=xx-elmer    Elmer Fudd
    hl=en          English
    hl=eo          Esperanto
    hl=et          Estonian
    hl=ee          Ewe
    hl=fo          Faroese
    hl=tl          Filipino
    hl=fi          Finnish
    hl=fr          French
    hl=fy          Frisian
    hl=gaa         Ga
    hl=gl          Galician
    hl=ka          Georgian
    hl=de          German
    hl=el          Greek
    hl=gn          Guarani
    hl=gu          Gujarati
    hl=xx-hacker   Hacker
    hl=ht          Haitian Creole
    hl=ha          Hausa
    hl=haw         Hawaiian
    hl=iw          Hebrew
    hl=hi          Hindi
    hl=hu          Hungarian
    hl=is          Icelandic
    hl=ig          Igbo
    hl=id          Indonesian
    hl=ia          Interlingua
    hl=ga          Irish
    hl=it          Italian
    hl=ja          Japanese
    hl=jw          Javanese
    hl=kn          Kannada
    hl=kk          Kazakh
    hl=rw          Kinyarwanda
    hl=rn          Kirundi
    hl=xx-klingon  Klingon
    hl=kg          Kongo
    hl=ko          Korean
    hl=kri         Krio (Sierra Leone)
    hl=ku          Kurdish
    hl=ckb         Kurdish (Soranî)
    hl=ky          Kyrgyz
    hl=lo          Laothian
    hl=la          Latin
    hl=lv          Latvian
    hl=ln          Lingala
    hl=lt          Lithuanian
    hl=loz         Lozi
    hl=lg          Luganda
    hl=ach         Luo
    hl=mk          Macedonian
    hl=mg          Malagasy
    hl=ms          Malay
    hl=ml          Malayalam
    hl=mt          Maltese
    hl=mi          Maori
    hl=mr          Marathi
    hl=mfe         Mauritian Creole
    hl=mo          Moldavian
    hl=mn          Mongolian
    hl=sr-ME       Montenegrin
    hl=ne          Nepali
    hl=pcm         Nigerian Pidgin
    hl=nso         Northern Sotho
    hl=no          Norwegian
    hl=nn          Norwegian (Nynorsk)
    hl=oc          Occitan
    hl=or          Oriya
    hl=om          Oromo
    hl=ps          Pashto
    hl=fa          Persian
    hl=xx-pirate   Pirate
    hl=pl          Polish
    hl=pt-BR       Portuguese (Brazil)
    hl=pt-PT       Portuguese (Portugal)
    hl=pa          Punjabi
    hl=qu          Quechua
    hl=ro          Romanian
    hl=rm          Romansh
    hl=nyn         Runyakitara
    hl=ru          Russian
    hl=gd          Scots Gaelic
    hl=sr          Serbian
    hl=sh          Serbo-Croatian
    hl=st          Sesotho
    hl=tn          Setswana
    hl=crs         Seychellois Creole
    hl=sn          Shona
    hl=sd          Sindhi
    hl=si          Sinhalese
    hl=sk          Slovak
    hl=sl          Slovenian
    hl=so          Somali
    hl=es          Spanish
    hl=es-419      Spanish (Latin American)
    hl=su          Sundanese
    hl=sw          Swahili
    hl=sv          Swedish
    hl=tg          Tajik
    hl=ta          Tamil
    hl=tt          Tatar
    hl=te          Telugu
    hl=th          Thai
    hl=ti          Tigrinya
    hl=to          Tonga
    hl=lua         Tshiluba
    hl=tum         Tumbuka
    hl=tr          Turkish
    hl=tk          Turkmen
    hl=tw          Twi
    hl=ug          Uighur
    hl=uk          Ukrainian
    hl=ur          Urdu
    hl=uz          Uzbek
    hl=vi          Vietnamese
    hl=cy          Welsh
    hl=wo          Wolof
    hl=xh          Xhosa
    hl=yi          Yiddish
    hl=yo          Yoruba
    hl=zu          Zulu
    */
}