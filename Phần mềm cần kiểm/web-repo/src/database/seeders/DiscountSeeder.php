<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use App\Models\Discount;
use DB;
class DiscountSeeder extends Seeder
{
    /**
    * Run the database seeds.
    */
    public function run(): void
    {
        // Thêm dữ liệu vào bảng brands
        DB::table('discounts')->insert([
            [
                'name' => 'Sale 100',
                'code' => 'SALE100',
                'price' => 100,
                'rank_id' => 1,
                'amount' => 100,
                'date_start' => '2023-11-01',
                'date_end' => '2050-12-31',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Sale 10000000',
                'code' => 'SALEOVERPRICE',
                'price' => 10000000,
                'rank_id' => 1,
                'amount' => 200,
                'date_start' => '2023-11-01',
                'date_end' => '2050-12-31',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Sale Expired',
                'code' => 'SALEEXPIRED',
                'price' => 100,
                'rank_id' => 1,
                'amount' => 200,
                'date_start' => '2023-11-01',
                'date_end' => '2023-12-01',
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Out Of Sale',
                'code' => 'OUTOFSALE',
                'price' => 100,
                'rank_id' => 1,
                'amount' => 0,
                'date_start' => '2023-11-01',
                'date_end' => '2050-12-31',
                'created_at' => now(),
                'updated_at' => now(),
            ]
        ]);
    }
}