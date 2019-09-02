// ======================================
// Author: Mahmood Abushaireh
// Email:  mabushaireh@outlook.com
// Copyright (c) 2017 www.mabushaireh.info
// 

// ======================================

import { Directive, Renderer, ElementRef, OnInit } from '@angular/core';


@Directive({
    selector: '[autofocus]'
})
export class AutofocusDirective implements OnInit {
    constructor(public renderer: Renderer, public elementRef: ElementRef) { }

    ngOnInit() {
        setTimeout(() => this.renderer.invokeElementMethod(this.elementRef.nativeElement, 'focus', []), 500);
    }
}