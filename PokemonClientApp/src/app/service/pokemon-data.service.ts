import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { PokemonData } from '../model/pokemon-data.model';

@Injectable({
  providedIn: 'root'
})
export class PokemonDataService {

  private baseurl = "https://localhost:7072/pokemon/tournament/statistics";
  constructor(private http : HttpClient) { }

  GetPokemonData(sortBy: string, sortDirection: string = 'asc') : Observable<PokemonData[]>
  {
     const params = new HttpParams()
     .set('sortBy', sortBy)
     .set('sortDirection', sortDirection);

     return this.http.get<PokemonData[]>(this.baseurl, {params}).pipe(catchError(error => {
      console.error("Failed to fetch pokemon Data :", error);
      return throwError(() => "Failed to fetch Pokemon Data");
     }))
  }
}