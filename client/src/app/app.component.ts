import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

//@component decorator that specifies the metadata for the component
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Skinet';
}
