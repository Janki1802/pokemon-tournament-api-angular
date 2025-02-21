import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PokemonTournamentStatisticsComponent } from './component/pokemon-tournament-statistics/pokemon-tournament-statistics.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PokemonTournamentStatisticsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'PokemonClientApp';
}
