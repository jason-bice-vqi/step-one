import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrls: ['./progress-bar.component.scss']
})
export class ProgressBarComponent {
  @Input() totalSteps: number = 1;
  @Input() completedSteps: number = 0;

  get stepsArray(): number[] {
    return Array.from({ length: this.totalSteps }, (_, i) => i + 1);
  }
}
