import { Directive, ElementRef, Renderer2, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[glowOnInit]'
})
export class GlowOnInitDirective implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    this.renderer.addClass(this.el.nativeElement, 'glow');
    setTimeout(() => {
      this.renderer.removeClass(this.el.nativeElement, 'glow');
    }, 1500); // duration should match animation duration
  }
}
