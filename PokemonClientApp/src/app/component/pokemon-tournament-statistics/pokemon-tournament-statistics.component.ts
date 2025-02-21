import { Component, OnDestroy, OnInit } from '@angular/core';
import { PokemonDataService } from '../../service/pokemon-data.service';
import { PokemonData } from '../../model/pokemon-data.model';
import { Subject, takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';
import {FormsModule} from'@angular/forms';

@Component({
  selector: 'app-pokemon-tournament-statistics',
  imports: [FormsModule, CommonModule],
  templateUrl: './pokemon-tournament-statistics.component.html',
  styleUrl: './pokemon-tournament-statistics.component.css'
})
export class PokemonTournamentStatisticsComponent implements OnInit, OnDestroy {
  sortBy: string = 'id';
  sortDirection: string='asc';
  pokemon: PokemonData[] = [];
  private destroy$ =  new Subject<void>();

  constructor(private service : PokemonDataService){}

  ngOnInit(){
   this.loadPokemonData();
  }

  loadPokemonData()
  {
     this.service.GetPokemonData(this.sortBy, this.sortDirection)
     .pipe(takeUntil(this.destroy$)).subscribe({
      next:(data : PokemonData[]) => {
        this.pokemon = data;
      },
      error : (error) => console.error("Error while loading pokemon data :", error)
     });
  }

  ngOnDestroy(){
   this.destroy$.next();
   this.destroy$.unsubscribe();
  }

}

