import { Pipe, Component, OnInit } from '@angular/core';
import { DashboardServiceProxy, DashboardDetailsDto } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
@Pipe({ name: 'safe' })
export class DashboardComponent implements OnInit {

  constructor(private DashboardServiceProxy: DashboardServiceProxy, protected route: ActivatedRoute, private sanitizer: DomSanitizer) { 
    this.getdashboardURL();
  }
  id: any;
  url: SafeResourceUrl;
  ngOnInit() {
  }
  getdashboardURL(){
    this.route.params.subscribe((params: any) => {
      if (params.id) {
          this.id = params.id;
          this.DashboardServiceProxy.getDashboard(this.id).subscribe(res => {
            console.log(res.embedUrl);
            this.url = this.sanitizer.bypassSecurityTrustResourceUrl(res.embedUrl);
          });
      }
  });
}
}
