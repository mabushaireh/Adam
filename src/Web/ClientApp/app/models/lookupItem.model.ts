// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

import { LocaleCategory, Language } from './enums';

export class LookupItem {
    // NOTE: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(id?: number, localeCategory?: LocaleCategory, lang?: Language, localeId?: number, localeString?: string) {

        this.id = id;
        this.localeCategory = localeCategory;
        this.lang = lang;
        this.localeId = localeId;
        this.localeString = localeString;

    }



    public id: number;
    public localeCategory: LocaleCategory;
    public lang: Language;
    public localeId: number;
    public localeString: string;
}