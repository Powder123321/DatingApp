import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import {
  GALLERY_CONFIG,
  GalleryConfig,
  GalleryItem,
  GalleryModule,
  ImageItem,
} from 'ng-gallery';
import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from 'src/app/app.component';

bootstrapApplication(AppComponent, {
  providers: [
    {
      provide: GALLERY_CONFIG,
      useValue: {
        autoHeight: true,
        imageSize: 'cover',
      } as GalleryConfig,
    },
  ],
});
@Component({
  selector: 'app-members-detail',
  standalone: true,
  templateUrl: './members-detail.component.html',
  styleUrls: ['./members-detail.component.css'],
  imports: [CommonModule, TabsModule, GalleryModule],
})
export class MembersDetailComponent implements OnInit {
  member: Member | undefined;
  images: GalleryItem[] = [];

  ngOnInit(): void {
    this.loadMember();
  }
  constructor(
    private memberService: MembersService,
    private route: ActivatedRoute
  ) {}

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;
    this.memberService.getMember(username).subscribe({
      next: (member) => {
        (this.member = member), this.getImages();
      },
    });
  }
  getImages() {
    if (!this.member) return;
    for (const photo of this.member.photos) {
      this.images.push(new ImageItem({ src: photo.url, thumb: photo.url }));
    }
  }
}
