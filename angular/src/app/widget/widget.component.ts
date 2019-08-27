import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'widget',
  template:
    `
      <div class="info-box {{widget_class}} hover-expand-effect">
        <div class="icon">
            <i class="material-icons">{{icon}}</i>
        </div>
        <div class="content">
            <div class="text">{{title}}</div>
            <div class="number">{{stats}}</div>
        </div>
      </div>
    `
})

export class WidgetComponent implements OnInit {

  @Input() widget_class:string;
  @Input() icon:string;
  @Input() title:string;
  @Input() stats:string;

  constructor() { }

  ngOnInit() {
  }

}
