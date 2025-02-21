import { TestBed } from '@angular/core/testing';
import { PokemonDataService } from './pokemon-data.service';
import { HttpClientTestingModule, HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { PokemonData } from '../model/pokemon-data.model';
import { provideHttpClient } from '@angular/common/http';

describe('PokemonDataService', () => {
  let service: PokemonDataService;
  let httpMock: HttpTestingController;

  const mockPokemonData: PokemonData[] = [
    { id: 1, name: 'Charizard', type: 'Fire', baseExperience: 200, wins: 80, losses: 20, ties: 10 },
    { id: 2, name: 'Squirtle', type: 'Water', baseExperience: 90, wins: 50, losses: 25, ties: 15 },
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [PokemonDataService, provideHttpClient(),provideHttpClientTesting()],
    });
    service = TestBed.inject(PokemonDataService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should fetch pokemon data successfully', () => {
    const sortBy = 'name';
    const sortDirection = 'asc';

    service.GetPokemonData(sortBy, sortDirection).subscribe((data) => {
      expect(data).toEqual(mockPokemonData);
    });

    const req = httpMock.expectOne('https://localhost:7072/pokemon/tournament/statistics?sortBy=name&sortDirection=asc');
    expect(req.request.method).toBe('GET');
    req.flush(mockPokemonData);
  });

  it('should handle error and return an error message', () => {
    const sortBy = 'name';
    const sortDirection = 'asc';

    service.GetPokemonData(sortBy, sortDirection).subscribe({
      next: () => fail('should have failed with an error'),
      error: (error) => {
        expect(error).toBe('Failed to fetch Pokemon Data');
      }
    });

    const req = httpMock.expectOne('https://localhost:7072/pokemon/tournament/statistics?sortBy=name&sortDirection=asc');
    expect(req.request.method).toBe('GET');
    req.flush('Error fetching data', { status: 500, statusText: 'Server Error' });
  });
});
