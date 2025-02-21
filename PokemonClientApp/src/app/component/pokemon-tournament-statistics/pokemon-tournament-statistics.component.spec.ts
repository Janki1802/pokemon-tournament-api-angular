import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PokemonTournamentStatisticsComponent } from './pokemon-tournament-statistics.component';
import { PokemonDataService } from '../../service/pokemon-data.service';
import { of, throwError } from 'rxjs';
import { PokemonData } from '../../model/pokemon-data.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

describe('PokemonTournamentStatisticsComponent', () => {
  let component: PokemonTournamentStatisticsComponent;
  let fixture: ComponentFixture<PokemonTournamentStatisticsComponent>;
  let pokemonDataService: jasmine.SpyObj<PokemonDataService>;

  const mockPokemonData: PokemonData[] = [
    {
      id: 1,
      name: 'Pikachu',
      type: 'Electric',
      baseExperience: 112,
      wins: 50,
      losses: 10,
      ties: 5,
    },
    {
      id: 2,
      name: 'Bulbasaur',
      type: 'Grass',
      baseExperience: 64,
      wins: 30,
      losses: 15,
      ties: 10,
    },
  ];

  beforeEach(async () => {
    // Create a spy object for PokemonDataService
    const serviceSpy = jasmine.createSpyObj('PokemonDataService', ['GetPokemonData']);

    await TestBed.configureTestingModule({
      imports: [PokemonTournamentStatisticsComponent, FormsModule, CommonModule],
      providers: [
        { provide: PokemonDataService, useValue: serviceSpy },
        provideHttpClient(), provideHttpClientTesting()
      ],
      
    }).compileComponents();

    pokemonDataService = TestBed.inject(PokemonDataService) as jasmine.SpyObj<PokemonDataService>;

    // Create the component fixture
    fixture = TestBed.createComponent(PokemonTournamentStatisticsComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load pokemon data on ngOnInit', () => {
    // Mock the service call
    pokemonDataService.GetPokemonData.and.returnValue(of(mockPokemonData));

    component.ngOnInit();

    expect(pokemonDataService.GetPokemonData).toHaveBeenCalledWith(component.sortBy, component.sortDirection);

    // Check if the pokemon list is populated correctly
    expect(component.pokemon).toEqual(mockPokemonData);
  });

  it('should handle error when loading pokemon data', () => {
    const errorResponse = 'Failed to fetch data';
    pokemonDataService.GetPokemonData.and.returnValue(throwError(() => new Error(errorResponse)));
  
    spyOn(console, 'error');
  
    component.ngOnInit();
  
    expect(component.pokemon.length).toBe(0);
  
    expect(console.error).toHaveBeenCalledWith('Error while loading pokemon data :', jasmine.any(Error));
  });
  

  it('should unsubscribe on ngOnDestroy', () => {
    spyOn(component['destroy$'], 'next');
    spyOn(component['destroy$'], 'unsubscribe');

    component.ngOnDestroy();

    expect(component['destroy$'].next).toHaveBeenCalled();
    expect(component['destroy$'].unsubscribe).toHaveBeenCalled();
  });
});
