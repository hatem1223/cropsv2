import { Pipe,PipeTransform, Component, OnInit,Injector } from '@angular/core';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { ReportServiceProxy, ReportDetailsDto } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})

@Pipe({ name: 'safe' })
export class ReportComponent implements OnInit {

  constructor(private ReportServiceProxy: ReportServiceProxy, protected route: ActivatedRoute, private sanitizer: DomSanitizer) { 
    this.getreportURL();
  }
  id: any;
  url: SafeResourceUrl;

  ngOnInit() {
  }

  getreportURL(){
    this.route.params.subscribe((params: any) => {
      if (params.id) {
          this.id = params.id;
          this.ReportServiceProxy.get(this.id).subscribe(res => {
            console.log(res.url);
            this.url = this.sanitizer.bypassSecurityTrustResourceUrl(res.url);
          });
      }
  });
  }
}
