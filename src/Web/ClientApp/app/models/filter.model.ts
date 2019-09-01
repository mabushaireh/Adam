
export class Filter {
    // NOTE: Using only optional constructor properties without backing store disables typescript's type checking for the type
    constructor(source: any, filterBy: string, filterValue: any) {
        this.filterBy = filterBy;
        this.filterValue = filterValue;
        this.source = source;
       
    }

    public filterBy: string;
    public filterValue: any
    public source: any;
}