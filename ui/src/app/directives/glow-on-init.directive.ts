import { Directive, ElementRef, Input, Renderer2, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[glowOnInit]'
})
export class GlowOnInitDirective implements AfterViewInit {
  @Input('glowOnInit') glowColor: string = 'blue';

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    const glowCssClass = `glow-${this.glowColor}`;

    this.renderer.addClass(this.el.nativeElement, glowCssClass);

    setTimeout(() => {
      this.renderer.removeClass(this.el.nativeElement, glowCssClass);
    }, 1500);
  }
}

