﻿using Desafio_Backend.Models;

namespace Desafio_Backend.Services
{
    public interface IAdminService
    {
        Task<Motorbike> AddMotorbike(Motorbike newBike);
    }
}